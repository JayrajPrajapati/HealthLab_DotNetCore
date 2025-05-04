using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.ApiViewModels.Merchant.Request;
using HealthLayby.Models.ApiViewModels.Merchant.Response;
using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Stripe;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// Merchant Service
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.IMerchantRepository" />
    public class MerchantService : BaseService, IMerchantRepository
    {
        #region Constructor

        /// <summary>
        /// The stripe application service
        /// </summary>
        private readonly IStripeAppService _stripeAppService;

        /// <summary>
        /// The general repository
        /// </summary>
        private readonly IGeneralRepository _generalRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="stripeAppService">The stripe application service.</param>
        /// <param name="generalRepository">The general repository.</param>
        public MerchantService(AppDbContext context, IStripeAppService stripeAppService, IGeneralRepository generalRepository) : base(context)
        {
            _stripeAppService = stripeAppService;
            _generalRepository = generalRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the merchant by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<MerchantModel?> GetMerchantByIdAsync(long id)
        {
            try
            {
                var merchantDetails = await (from m in _context.Merchant
                                             join mb in _context.MerchantBank on m.MerchantId equals mb.MerchantId
                                             where m.MerchantId == id
                                             && !m.IsDeleted
                                             && !mb.IsDeleted
                                             select new MerchantModel
                                             {
                                                 MerchantId = m.MerchantId,
                                                 FullName = m.FullName,
                                                 FirstName = m.FirstName,
                                                 LastName = m.LastName,
                                                 EmailAddress = m.EmailAddress,
                                                 IsActive = m.IsActive,
                                                 ProfilePic = m.ProfilePic,
                                                 Logo = m.Logo,
                                                 PhoneNumber = m.PhoneNumber,
                                                 BankName = mb.BankName,
                                                 BranchName = mb.BankLocation,
                                                 BankAccountNo = mb.AccountNumber,
                                                 BSBNo = mb.BSB,
                                                 Password = m.Password,
                                                 CategoryId = m.CategoryId,
                                                 ServiceIds = m.ServiceId,
                                                 CreatedOn = m.CreatedOn,
                                                 Accepted = m.Accepted
                                             }).FirstOrDefaultAsync();
                if (merchantDetails != null && merchantDetails.MerchantId > 0)
                {
                    var clinic = _context.Clinic.Where(x => x.MerchantId == merchantDetails.MerchantId && !x.IsDeleted && x.IsActive == true).FirstOrDefault();
                    if (clinic is not null)
                    {
                        merchantDetails.ClinicName = string.IsNullOrWhiteSpace(clinic.ClinicName) ? string.Empty : clinic.ClinicName;
                        merchantDetails.ClinicLocation = string.IsNullOrWhiteSpace(clinic.ClinicLocation) ? string.Empty : clinic.ClinicLocation;
                    }

                    merchantDetails.CategoryList = await _generalRepository.GetActiveCategoryDropdownListAsync();

                    merchantDetails.ServiceList = await _generalRepository.GetActiveServiceDropdownListAsync(merchantDetails.CategoryId);

                    if (merchantDetails.CategoryId > 0)
                    {
                        merchantDetails.CategoryName = await _context.Category.Where(x => x.CategoryId == merchantDetails.CategoryId).Select(x => x.CategoryName).FirstOrDefaultAsync() ?? string.Empty;
                    }
                    if (!string.IsNullOrWhiteSpace(merchantDetails.ServiceIds))
                    {
                        var ServiceIds = merchantDetails.ServiceIds.Split(',');
                        if (ServiceIds.Length > 0)
                        {
                            for (int i = 0; i < ServiceIds.Length; i++)
                            {
                                if (i == 0)
                                {
                                    merchantDetails.ServicesNames += await _context.Service.Where(x => x.ServiceId == Convert.ToInt64(ServiceIds[i])).Select(x => x.ServiceName).FirstOrDefaultAsync();
                                }
                                else
                                {
                                    merchantDetails.ServicesNames += ", " + await _context.Service.Where(x => x.ServiceId == Convert.ToInt64(ServiceIds[i])).Select(x => x.ServiceName).FirstOrDefaultAsync();
                                }
                            }
                        }
                    }
                }

                if (merchantDetails == null)
                {
                    merchantDetails = new MerchantModel
                    {
                        IsActive = true,
                        CategoryList = await _generalRepository.GetActiveCategoryDropdownListAsync(),
                        ServiceList = await _context.Service.Where(x => x.IsActive == true && !x.IsDeleted).Select(x => new SelectListItem
                        {
                            Text = x.ServiceName,
                            Value = x.ServiceId.ToString()
                        }).ToListAsync()
                    };
                    return merchantDetails;
                };
                return merchantDetails;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the merchant list.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public async Task<Tuple<List<MerchantGridListResult>, int, int>> GetMerchantListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText)
        {
            try
            {
                var paramTotalRecords = new OutputParameter<int?>();
                var paramTotalFilteredRecords = new OutputParameter<int?>();

                var result = await _context.GetProcedures().MerchantGridListAsync
                (
                    SortColumn: sortColumn,
                    SortOrder: sortOrder,
                    PageSize: pageSize,
                    PageIndex: pageIndex,
                    SearchText: searchText,
                    TotalRecords: paramTotalRecords,
                    TotalFilteredRecords: paramTotalFilteredRecords
                );

                return Tuple.Create(result, paramTotalRecords.Value ?? 0, paramTotalFilteredRecords.Value ?? 0);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Saves the merchant asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveMerchantAsync(MerchantModel model, long adminId, string? password = null)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                if (await IsMerchantEmailExistAsync(model.MerchantId, model.EmailAddress))
                {
                    return (false, MessageConstant.EmailAlredyExist);
                }

                var message = string.Empty;
                if (model.MerchantId > 0)
                {
                    var merchant = await _context.Merchant.FirstOrDefaultAsync(q => q.MerchantId == model.MerchantId && !q.IsDeleted);

                    if (merchant is null)
                    {
                        return (false, MessageConstant.MerchantNotFound);
                    }

                    merchant.FirstName = model.FirstName;
                    merchant.LastName = model.LastName;
                    merchant.EmailAddress = model.EmailAddress;
                    merchant.PhoneNumber = model.PhoneNumber;
                    merchant.IsActive = model.IsActive;
                    merchant.ProfilePic = !string.IsNullOrWhiteSpace(model.ProfilePic) ? model.ProfilePic : string.Empty;
                    merchant.Logo = !string.IsNullOrWhiteSpace(model.Logo) ? model.Logo : string.Empty;
                    merchant.UpdatedOn = DateTime.UtcNow;
                    merchant.UpdatedBy = adminId;
                    merchant.CategoryId = model.CategoryId;
                    merchant.ServiceId = model.ServiceIds;
                    _context.Merchant.Update(merchant);

                    var merchantBank = await _context.MerchantBank.FirstOrDefaultAsync(q => q.MerchantId == model.MerchantId && !q.IsDeleted);

                    if (merchantBank is null)
                    {
                        return (false, MessageConstant.MerchantBankNotFound);
                    }

                    merchantBank.BankName = model.BankName;
                    merchantBank.BankLocation = model.BranchName;
                    merchantBank.AccountNumber = model.BankAccountNo;
                    merchantBank.BSB = model.BSBNo;
                    merchantBank.UpdatedOn = DateTime.UtcNow;
                    merchantBank.UpdatedBy = adminId;

                    _context.MerchantBank.Update(merchantBank);

                    var clinic = await _context.Clinic.FirstOrDefaultAsync(q => q.MerchantId == model.MerchantId && !q.IsDeleted);
                    if (merchantBank is null)
                    {
                        return (false, MessageConstant.ClinicNotFound);
                    }

                    clinic.ClinicName = model.ClinicName;
                    clinic.ClinicLocation = model.ClinicLocation;
                    clinic.UpdatedOn = DateTime.UtcNow;
                    clinic.UpdatedBy = adminId;
                    clinic.IsActive = true;
                    _context.Clinic.Update(clinic);

                    await _stripeAppService.UpdateStripeCustomerAsync
                    (
                        stripeCustomerId: merchant.StripeId,
                        customer: new CustomerUpdateOptions
                        {
                            Email = model.EmailAddress,
                            Name = $"{model.FirstName} {model.LastName}"
                        }
                    );

                    await _context.SaveChangesAsync();
                    message = MessageConstant.UpdateMerchantSuccess;
                }
                else
                {
                    var stripeCustomer = await _stripeAppService.AddStripeCustomerAsync(new Stripe.CustomerCreateOptions
                    {
                        Email = model.EmailAddress,
                        Name = $"{model.FirstName} {model.LastName}"
                    });

                    var merchant = new Merchant
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        EmailAddress = model.EmailAddress,
                        PhoneNumber = model.PhoneNumber,
                        IsActive = model.IsActive,
                        ProfilePic = model.ProfilePic,
                        Logo = model.Logo,
                        Password = password ?? CommonLogic.Encrypt(Password.Generate(10, 4)),
                        Status = (int)MerchantRequestEnum.Accepted,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = adminId,
                        StripeId = stripeCustomer.CustomerId,
                        ClinicId = 1,
                        Notes = string.Empty,
                        CategoryId = model.CategoryId,
                        ServiceId = model.ServiceIds
                    };

                    await _context.Merchant.AddAsync(merchant);
                    await _context.SaveChangesAsync();

                    if (merchant is not null)
                    {
                        await _context.MerchantBank.AddAsync(new MerchantBank
                        {
                            MerchantId = merchant.MerchantId,
                            BankName = model.BankName,
                            BankLocation = model.BranchName,
                            AccountNumber = model.BankAccountNo,
                            BSB = model.BSBNo,
                            IsVerified = true,
                            CreatedOn = DateTime.Now,
                            CreatedBy = adminId
                        });

                        await _context.Clinic.AddAsync(new Clinic
                        {
                            MerchantId = merchant.MerchantId,
                            ClinicName = model.ClinicName,
                            ClinicLocation = model.ClinicLocation,
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = adminId,
                            IsActive = true
                        });
                        await _context.SaveChangesAsync();
                    }
                    message = MessageConstant.CreateMerchantSuccess;
                }
                await trans.CommitAsync();
                return (true, message);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Deletes the merchant asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> DeleteMerchantAsync(long id, long adminId)
        {
            try
            {
                //var isMerchantAvailableInService = await _context.Service.AnyAsync(q => q.MerchantId == id && !q.IsDeleted);

                //if (isMerchantAvailableInService)
                //{
                //    return Tuple.Create(false, MessageConstant.MerchantUsedInServiceFail);
                //}

                var merchant = await _context.Merchant.FirstOrDefaultAsync(q => q.MerchantId == id && !q.IsDeleted);
                if (merchant is null)
                {
                    return Tuple.Create(false, MessageConstant.MerchantNotFound);
                }

                merchant.IsDeleted = true;
                merchant.DeletedOn = DateTime.UtcNow;
                merchant.DeletedBy = adminId;

                _context.Merchant.Update(merchant);
                var merchantBank = await _context.MerchantBank.FirstOrDefaultAsync(q => q.MerchantId == id && !q.IsDeleted);

                if (merchantBank is null)
                {
                    return Tuple.Create(false, MessageConstant.MerchantBankNotFound);
                }

                merchantBank.IsDeleted = true;
                merchantBank.DeletedOn = DateTime.UtcNow;
                merchantBank.DeletedBy = adminId;

                _context.MerchantBank.Update(merchantBank);
                await _context.SaveChangesAsync();
                return Tuple.Create(true, MessageConstant.DeleteMerchantSuccess);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is merchant email exist asynchronous] [the specified merchantid].
        /// </summary>
        /// <param name="merchantid">The merchantid.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public async Task<bool> IsMerchantEmailExistAsync(long? merchantid, string email)
        {
            try
            {
                return await _context.Merchant.AnyAsync(x => x.EmailAddress == email
                                                          && x.IsDeleted == false
                                                          && x.MerchantId != merchantid);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the name of the merchant.
        /// </summary>
        /// <returns></returns>
        public async Task<List<MerchantModel>> GetMerchantListAsync()
        {
            try
            {
                return await _context.Merchant.Where(x => x.IsActive && !x.IsDeleted)
                                              .Select(x => new MerchantModel
                                              {
                                                  MerchantId = x.MerchantId,
                                                  FullName = x.FullName,
                                              }).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Merchants the grid top record list asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TopMerchantGridListResult>> TopMerchantGridListAsync()
        {
            try
            {
                var result = await _context.GetProcedures().TopMerchantGridListAsync();

                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the merchant total count asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetMerchantCountAsync()
        {
            try
            {
                return await _context.Merchant.CountAsync(x => !x.IsDeleted && x.Status == (int)MerchantRequestEnum.Accepted);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the merchant.
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, int>> GetMerchantChartDataAsync()
        {
            try
            {
                var result = new Dictionary<string, int>();

                var currentDateTime = DateTime.Now;
                var fromDate = currentDateTime.AddMonths(-6);
                var merchantCreatedDates = await _context.Merchant.Where(q => !q.IsDeleted
                                                                           && q.CreatedOn.Date >= fromDate.Date
                                                                        )
                                                                  .Select(q => q.CreatedOn)
                                                                  .ToListAsync();

                for (var count = 0; count <= 5; count++)
                {
                    var monthDate = currentDateTime.AddMonths(-count);

                    var monthCount = merchantCreatedDates.Count(x => x.Month == monthDate.Month
                                                                  && x.Year == monthDate.Year
                                                               );

                    result.Add
                    (
                        $"{monthDate.ToString("MMM")}-{monthDate.Year}",
                        monthCount
                    );
                }

                return result.Reverse().ToDictionary(x => x.Key, x => x.Value);
            }
            catch
            {
                throw;
            }
        }

        #region For API's

        /// <summary>
        /// Gets the merchant list.
        /// </summary>
        /// <param name="merchentListingRequest">The merchent listing request.</param>
        /// <returns></returns>
        public async Task<(bool, string, List<MerchantListingResponse>)> GetMerchantListForAPI(MerchentListingRequest merchentListingRequest)
        {
            List<MerchantListingResponse> merchantListingResponseList = new List<MerchantListingResponse>();
            MerchantListingResponse merchantListingResponse = new MerchantListingResponse();
            List<Merchant> merchantListing = new List<Merchant>();

            var message = string.Empty;
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                if (merchentListingRequest.sortBy != null && merchentListingRequest.sortBy.ToLower().Contains("desc"))
                {
                    merchantListing = await _context.Merchant.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.FullName).ToListAsync();
                }
                else if (merchentListingRequest.sortBy != null && merchentListingRequest.sortBy.ToLower().Contains("new"))
                {
                    merchantListing = await _context.Merchant.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.MerchantId).ToListAsync();
                }
                else if (merchentListingRequest.sortBy != null && merchentListingRequest.sortBy.ToLower().Contains("asc"))
                {
                    merchantListing = await _context.Merchant.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.FullName).ToListAsync();
                }
                else
                    merchantListing = await _context.Merchant.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.MerchantId).ToListAsync();

                if (merchantListing != null)
                {
                    foreach (var list in merchantListing)
                    {
                        merchantListingResponse = new MerchantListingResponse();

                        merchantListingResponse.MerchantId = list.MerchantId;
                        merchantListingResponse.FullName = list.FullName;
                        merchantListingResponse.ProfilePic = list.ProfilePic;
                        merchantListingResponse.Logo = list.Logo;
                        merchantListingResponse.CreatedOn = list.CreatedOn;

                        merchantListingResponseList.Add(merchantListingResponse);
                    }

                    message = ApiMessages.SuccessfullMerchantList;

                    if (merchantListingResponseList.Count > 0)
                        return (true, message, merchantListingResponseList);
                    else
                        return (false, ApiMessages.InvalidMerchantList, null);
                }
                else
                {
                    return (false, ApiMessages.InvalidMerchantList, null);
                }
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        #endregion

        #endregion
    }
}
