using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.Context;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Runtime.InteropServices;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Repositories.Services.MerchantServices
{
    /// <summary>
    /// MerchantWebService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.MerchantRepositories.IMerchantWebRepository" />
    public class MerchantWebService : BaseService, IMerchantWebRepository
    {
        #region Private Variable 

        /// <summary>
        /// The stripe application service
        /// </summary>
        private readonly IStripeAppService _stripeAppService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantWebService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="stripeAppService">The stripe application service.</param>
        public MerchantWebService(AppDbContext context, IStripeAppService stripeAppService) : base(context)
        {
            _stripeAppService = stripeAppService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the merchant by email address asynchronous.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        public async Task<Merchant?> GetMerchantByEmailAddressAsync(string emailAddress)
        {
            try
            {
                return await _context.Merchant.Where(x => x.EmailAddress == emailAddress && x.IsActive == true && x.IsRejected == false && x.IsDeleted == false)
                                           .Select(q => new Merchant
                                           {
                                               MerchantId = q.MerchantId,
                                               EmailAddress = q.EmailAddress,
                                               FirstName = q.FirstName,
                                               FullName = q.FullName,
                                               LastName = q.LastName,
                                               Password = q.Password,
                                               ClinicId = q.ClinicId,
                                               CategoryId = q.CategoryId,
                                               ServiceId = q.ServiceId,
                                               Status = q.Status
                                           }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the unused token for merchant asynchronous.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public async Task<Guid?> GetUnusedTokenForMerchantAsync(long merchantId)
        {
            try
            {
                return await _context.ForgotPasswordToken.Where(x => x.AdminId.HasValue
                                                                  && x.MerchantId.Value == merchantId
                                                                  && !x.ChangedOn.HasValue
                                                               )
                                                         .Select(x => x.Token)
                                                         .FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adds the forgot password token for merchant asynchronous.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<bool> AddForgotPasswordTokenForMerchantAsync(long merchantId, Guid token)
        {
            try
            {
                var model = new ForgotPasswordToken
                {
                    MerchantId = merchantId,
                    Token = token,
                    CreatedOn = DateTime.UtcNow
                };
                await _context.ForgotPasswordToken.AddAsync(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Checks the merchant token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<bool> CheckMerchantTokenAsync(Guid token)
        {
            try
            {
                return await _context.ForgotPasswordToken.AnyAsync(q => q.Token == token && q.MerchantId.HasValue && !q.ChangedOn.HasValue);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the merchant identifier by token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<long?> GetMerchantIdByTokenAsync(Guid token)
        {
            try
            {
                return await _context.ForgotPasswordToken.Where(x => x.Token == token
                                                                  && x.MerchantId.HasValue
                                                               )
                                                         .Select(x => x.MerchantId)
                                                         .FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the merchant reset password asynchronous.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<bool> UpdateMerchantResetPasswordAsync(long merchantId, string password)
        {
            try
            {
                var merchant = await _context.Merchant.FirstOrDefaultAsync(x => x.MerchantId == merchantId);

                if (merchant is null)
                {
                    return false;
                }

                merchant.Password = password;

                _context.Merchant.Update(merchant);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the merchant by identifier asynchronous.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public async Task<MerchantModel?> GetMerchantByIdAsync(long merchantId)
        {
            try
            {
                return await _context.Merchant.Where(x => x.MerchantId == merchantId)
                                           .Select(q => new MerchantModel
                                           {
                                               MerchantId = q.MerchantId,
                                               EmailAddress = q.EmailAddress,
                                               FirstName = q.FirstName,
                                               FullName = q.FullName,
                                               LastName = q.LastName,
                                               Password = q.Password
                                           }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the merchant change date asynchronous.
        /// </summary>
        /// <param name="Token">The token.</param>
        /// <returns></returns>
        public async Task<bool> UpdateMerchantChangeDateAsync(Guid Token)
        {
            try
            {
                var forgotPasswordTokenDate = await _context.ForgotPasswordToken.Where(x => x.Token == Token)
                                           .FirstOrDefaultAsync();
                if (forgotPasswordTokenDate is not null)
                {
                    forgotPasswordTokenDate.ChangedOn = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                throw;
            }
            return false;
        }

        /// <summary>
        /// Determines whether [is merchant exists] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="firstname">The firstname.</param>
        /// <returns>
        ///   <c>true</c> if [is merchant exists] [the specified email]; otherwise, <c>false</c>.
        /// </returns>
        public async Task<long> IsMerchantExists(string email, string firstname)
        {
            var isMerchantExistsRejected = await _context.Merchant.Where(x => x.EmailAddress == email && x.FirstName == firstname && x.IsRejected == true).FirstOrDefaultAsync();
            
            if (isMerchantExistsRejected is not null)
                return isMerchantExistsRejected.MerchantId;
            else 
            {
                isMerchantExistsRejected = await _context.Merchant.Where(x => x.EmailAddress == email && x.FirstName == firstname && x.IsDeleted == false).FirstOrDefaultAsync();

                if (isMerchantExistsRejected is not null) 
                    return isMerchantExistsRejected.MerchantId;
                else
                    return 0; 
            }
        }

        /// <summary>
        /// Saves the merchant asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<(bool, string,Merchant)> SaveMerchantAsync(RegisterMerchantModel model, long adminId, string password)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                if (await GetMerchantByEmailAddressAsync(model.Email) is not null)
                {
                    return (false, MessageConstant.EmailAlredyExist,null);
                }

                var message = string.Empty; string number = string.Empty;
                var stripeCustomer = await _stripeAppService.AddStripeCustomerAsync(new Stripe.CustomerCreateOptions
                {
                    Email = model.Email,
                    Name = $"{model.FirstName} {model.LastName}"
                });

                if(model.ServiceId.Length > 0)
                {
                    for(int i=0; i < model.ServiceId.Length; i++)
                    {
                        if (i == 0)
                        {
                            number = model.ServiceId[i];
                        }
                        else
                        {
                            number = number + "," + model.ServiceId[i];
                        }
                    }
                }

                var merchant = new Merchant
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    FullName = model.FirstName + " " + model.LastName,
                    EmailAddress = model.Email,
                    PhoneNumber = model.Phone,
                    IsActive = false,
                    Password = password ?? CommonLogic.Encrypt(Password.Generate(10, 4)),
                    CategoryId =  model.Category,
                    ClinicId=0,
                    ServiceId = number,
                    Notes = model.Notes,
                    //Status = (int)MerchantRequestEnum.Pending,
                    Status = 0,
                    //IsRejected == false,
                    IsDeleted = false,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = adminId,
                    StripeId = stripeCustomer.CustomerId
                };

                await _context.Merchant.AddAsync(merchant);
                await _context.SaveChangesAsync();

                if (merchant is not null)
                {
                    var clinic = new Clinic
                    {
                        MerchantId = merchant.MerchantId,
                        ClinicName= model.ClinicName,
                        IsActive = false,
                        CreatedBy=adminId,
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    await _context.Clinic.AddAsync(clinic);
                    await _context.SaveChangesAsync();     
                    
                    if(clinic is not null)
                    {
                      Merchant merchantUpdate = await _context.Merchant.Where(z => z.MerchantId == merchant.MerchantId).FirstOrDefaultAsync();
                      merchantUpdate.ClinicId = clinic.ClinicId;

                        _context.Merchant.Update(merchantUpdate);
                        await _context.SaveChangesAsync();
                    }
                }

                message = MessageConstant.CreateMerchantSuccess;
                await trans.CommitAsync();
                return (true, message,merchant);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Gets the active category dropdown list asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetActiveCategoryDropdownListAsync()
        {
            try
            {
                return await _context.Category.Where(q => !q.IsDeleted && q.IsActive)
                                              .Select(q => new SelectListItem
                                              {
                                                  Value = q.CategoryId.ToString(),
                                                  Text = q.CategoryName
                                              }).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the active service dropdown list asynchronous.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetActiveServiceDropdownListAsync(long categoryId)
        {
            try
            {
                return await _context.Service.Where(q => q.CategoryId == categoryId && !q.IsDeleted && q.IsActive)
                                              .Select(q => new SelectListItem
                                              {
                                                  Value = q.ServiceId.ToString(),
                                                  Text = q.ServiceName
                                              }).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adds the generated otp.
        /// </summary>
        /// <param name="randomOTP">The random otp.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<bool> AddGeneratedOTP(string randomOTP, Guid token)
        {
            try
            {
                var model = new GenerateOTP
                {
                    GUID = token,
                    Otp = Convert.ToInt64(randomOTP),
                    IsUsed = false,
                    CreatedBy = 0,
                    CreatedOn = DateTime.UtcNow
                };
                await _context.GenerateOTP.AddAsync(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// Gets the generated otp.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<long> GetGeneratedOTP(Guid token)
        {
            try
            {
                var getOTP = await _context.GenerateOTP.Where(x => x.GUID == token && x.IsUsed == false).FirstOrDefaultAsync();
                if (getOTP is not null)
                    return getOTP.Otp;
                else
                    return 0;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updateds the otp.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<bool> UpdatedOTP(Guid token)
        {
            try
            {
                var getOTP = await _context.GenerateOTP.Where(x => x.GUID == token && x.IsUsed == false).FirstOrDefaultAsync();
                if (getOTP is not null)
                {
                    getOTP.IsUsed = true;

                    _context.GenerateOTP.Update(getOTP);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the merchant clinic by identifier asynchronous.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="otp">The otp.</param>
        /// <returns></returns>
        public async Task<bool> UpdateMerchantClinicByIdAsync(long merchantId,long otp)
        {
            try
            {
                var merchant = await _context.Merchant.Where(x => x.MerchantId == merchantId && x.IsDeleted == false && x.IsRejected == false).FirstOrDefaultAsync();
                if (merchant is not null)
                {
                    merchant.IsActive = true;
                    merchant.UpdatedBy = merchant.CreatedBy;
                    merchant.UpdatedOn = DateTime.UtcNow;

                    _context.Merchant.Update(merchant);
                    await _context.SaveChangesAsync();

                    var clinic = await _context.Clinic.Where(y => y.MerchantId == merchant.MerchantId && y.IsActive == false).FirstOrDefaultAsync();
                    if(clinic is not null)
                    {
                        clinic.IsActive = true;
                        clinic.UpdatedBy = clinic.CreatedBy;
                        clinic.UpdatedOn = DateTime.UtcNow;

                        _context.Clinic.Update(clinic);
                        await _context.SaveChangesAsync();


                        var otpUpdateStatus = await _context.GenerateOTP.Where(z => z.Otp == otp && z.IsUsed == false).FirstOrDefaultAsync();
                        if (otpUpdateStatus is not null)
                        {
                            otpUpdateStatus.IsUsed = true;
                            _context.GenerateOTP.Update(otpUpdateStatus);
                            await _context.SaveChangesAsync();

                            return true;
                        }
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adds the merchant bank.
        /// </summary>
        /// <param name="merchantBankDetails">The merchant bank details.</param>
        /// <returns></returns>
        public async Task<bool> AddMerchantBank(MerchantBankDetails merchantBankDetails)
        {
            try
            {
                var model = new MerchantBank
                {
                    MerchantId = merchantBankDetails.merchantId,
                    BankName = merchantBankDetails.BankName,
                    BankLocation = merchantBankDetails.BankLocation,
                    AccountNumber = merchantBankDetails.BankAccountNumber,
                    BSB = merchantBankDetails.BSBNumber,
                    IsVerified = true,
                    CreatedBy = merchantBankDetails.merchantId,
                    CreatedOn = DateTime.UtcNow
                };
                await _context.MerchantBank.AddAsync(model);
                await _context.SaveChangesAsync();

                if(model.MerchantBankId > 0) 
                {
                    var merchant = await _context.Merchant.Where(x => x.MerchantId == model.MerchantId && x.Status == 0).FirstOrDefaultAsync();
                    if(merchant is not null)
                    {
                        merchant.Status = (int)MerchantRequestEnum.Pending;

                        _context.Merchant.Update(merchant);
                        await _context.SaveChangesAsync();

                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }
        #endregion

    }
}