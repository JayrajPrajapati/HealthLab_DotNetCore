using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// merchantRequestService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.IMerchantRequestRepository" />
    public class MerchantRequestService : BaseService, IMerchantRequestRepository
    {

        #region Private Variable
        /// <summary>
        /// The general repository
        /// </summary>
        private readonly IGeneralRepository _generalRepository;
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantRequestService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="generalRepository">The general repository.</param>
        public MerchantRequestService(AppDbContext context, IGeneralRepository generalRepository) : base(context)
        {
            _generalRepository = generalRepository;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Gets the merchant request by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<MerchantModel?> GetMerchantRequestByIdAsync(long id)
        {
            try
            {
                var merchantRequestModel = await (from m in _context.Merchant
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
                                                      CreatedOn = m.CreatedOn
                                                  }).FirstOrDefaultAsync();

                if (merchantRequestModel != null && merchantRequestModel.MerchantId > 0)
                {
                    var clinic = _context.Clinic.Where(x => x.MerchantId == merchantRequestModel.MerchantId && !x.IsDeleted && x.IsActive == true).FirstOrDefault();
                    if (clinic is not null)
                    {
                        merchantRequestModel.ClinicName = string.IsNullOrWhiteSpace(clinic.ClinicName) ? string.Empty : clinic.ClinicName;
                        merchantRequestModel.ClinicLocation = string.IsNullOrWhiteSpace(clinic.ClinicLocation) ? string.Empty : clinic.ClinicLocation;
                    }

                    if (merchantRequestModel.CategoryId > 0)
                    {
                        merchantRequestModel.CategoryName = await _context.Category.Where(x => x.CategoryId == merchantRequestModel.CategoryId).Select(x => x.CategoryName).FirstOrDefaultAsync() ?? string.Empty;
                    }
                    if (!string.IsNullOrWhiteSpace(merchantRequestModel.ServiceIds))
                    {
                        var ServiceIds = merchantRequestModel.ServiceIds.Split(',');
                        if (ServiceIds.Length > 0)
                        {
                            for (int i = 0; i < ServiceIds.Length; i++)
                            {
                                if (i == 0)
                                {
                                    merchantRequestModel.ServicesNames += await _context.Service.Where(x => x.ServiceId == Convert.ToInt64(ServiceIds[i])).Select(x => x.ServiceName).FirstOrDefaultAsync();
                                }
                                else
                                {
                                    merchantRequestModel.ServicesNames += ", " + await _context.Service.Where(x => x.ServiceId == Convert.ToInt64(ServiceIds[i])).Select(x => x.ServiceName).FirstOrDefaultAsync();
                                }
                            }
                        }
                    }
                }
                return merchantRequestModel;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the merchant request by identifier for reject request asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<MerchantRequestModel?> GetMerchantRequestByIdForRejectRequestAsync(long id)
        {
            try
            {
                var merchantRequestModel = await _context.Merchant.Where(x => x.MerchantId == id && !x.IsDeleted)
                                                .Select(x => new MerchantRequestModel
                                                {
                                                    MerchantId = x.MerchantId,
                                                    CategoryId = x.CategoryId,
                                                    ServiceIds = x.ServiceId,
                                                    ServiceId = x.ServiceId
                                                }).FirstOrDefaultAsync();
                if (merchantRequestModel != null)
                {
                    merchantRequestModel.CategoryList = await _generalRepository.GetActiveCategoryDropdownListAsync();
                }
                return merchantRequestModel;

            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the merchant request list.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public async Task<Tuple<List<MerchantRequestGridListResult>, int, int>> GetMerchantRequestListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText)
        {
            try
            {
                var paramTotalRecords = new OutputParameter<int?>();
                var paramTotalFilteredRecords = new OutputParameter<int?>();

                var result = await _context.GetProcedures().MerchantRequestGridListAsync
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
        /// Rejects the merchant request asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="merchantRequestenum">The merchant requestenum.</param>
        /// <param name="rejectedReason">The rejected reason.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="servicesIds">The services ids.</param>
        /// <returns></returns>
        public async Task<(bool, string)> AcceptAndRejectMerchantRequestAsync(long id, long adminId, MerchantRequestEnum merchantRequestenum, string rejectedReason, long categoryId = 0, string servicesIds = "")
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var merchant = await _context.Merchant.FirstOrDefaultAsync(q => q.MerchantId == id && !q.IsDeleted);

                if (merchant is null)
                {
                    return (false, MessageConstant.MerchantNotFound);
                }

                if ((int)merchantRequestenum == 3)
                {
                    merchant.Status = (int)merchantRequestenum;
                    merchant.CategoryId = categoryId;
                    merchant.ServiceId = servicesIds;
                    merchant.UpdatedOn = DateTime.UtcNow;
                    merchant.UpdatedBy = adminId;
                    merchant.Accepted = DateTime.UtcNow;
                    merchant.IsActive = true;
                }
                else if ((int)merchantRequestenum == 2)
                {
                    merchant.Status = (int)merchantRequestenum;
                    merchant.RejectReason = rejectedReason;
                    merchant.UpdatedOn = DateTime.UtcNow;
                    merchant.UpdatedBy = adminId;
                }

                _context.Merchant.Update(merchant);
                await _context.SaveChangesAsync();

                await trans.CommitAsync();

                if (merchantRequestenum == MerchantRequestEnum.Accepted)
                {
                    return (true, MessageConstant.AcceptMerchantRequest);
                }
                else
                {
                    return (true, MessageConstant.RejectMerchantRequestSuccess);
                }
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Gets the merchant full name and email by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<MerchantModel?> GetMerchantFullNameAndEmailByIdAsync(long id)
        {
            try
            {
                return await _context.Merchant.Where(q => q.MerchantId == id && !q.IsDeleted)
                                               .Select(q => new MerchantModel
                                               {
                                                   FullName = q.FullName,
                                                   EmailAddress = q.EmailAddress
                                               }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }

        }

        #endregion
    }
}
