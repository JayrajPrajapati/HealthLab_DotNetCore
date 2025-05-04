using HealthLayby.Models.Context;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace HealthLayby.Repositories.Services.MerchantServices
{
    public class MerchantProfileService : BaseService, IMerchantProfileWebRepository
    {
        #region Private Variable 
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantProfileService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MerchantProfileService(AppDbContext context) : base(context)
        {

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Merchents the profile model.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public MerchentProfileModel GetMerchantProfile(long merchantId)
        {
            try
            {
                MerchentProfileModel merchentProfileModel = new MerchentProfileModel();

                var merchant = _context.Merchant.Where(x => x.MerchantId == merchantId).FirstOrDefault();
                if (merchant is not null)
                {
                    merchentProfileModel.profileImage = merchant.ProfilePic;
                    merchentProfileModel.firstName = merchant.FirstName;
                    merchentProfileModel.lastName = merchant.LastName;
                    merchentProfileModel.emailAddress = merchant.EmailAddress;
                    merchentProfileModel.phoneNumber = merchant.PhoneNumber;

                    //merchentProfileModel.clinics = _context.Clinic.Where(y => y.MerchantId == merchant.MerchantId && y.IsActive == true && !y.IsDeleted).ToList();

                    var clinic = _context.Clinic.Where(y => y.MerchantId == merchant.MerchantId && y.IsActive == true && !y.IsDeleted).FirstOrDefault();
                    if(clinic is not null)
                    {
                        merchentProfileModel.clinicName = clinic.ClinicName;
                        merchentProfileModel.clinicLocation = clinic.ClinicLocation;
                        merchentProfileModel.Logo = clinic.ClinicImage;
                    }

                    var merchantBank = _context.MerchantBank.Where(z => z.MerchantId == merchant.MerchantId && !z.IsDeleted).FirstOrDefault();
                    if(merchantBank is not null)
                    {                        
                        merchentProfileModel.accountNumber = merchantBank.AccountNumber;
                        merchentProfileModel.bsbNumber = merchantBank.BSB;

                        var location = _context.BankBSBDirectory.Where(i => i.BSBNumber == merchantBank.BSB).FirstOrDefault();
                        if(location is not null) 
                        { 
                            merchentProfileModel.bankLocation = location.BankStreetAddress;

                            var bName = _context.Bank.Where(q => q.BankId == location.BankId).FirstOrDefault();
                            if(bName is not null)
                                merchentProfileModel.bankName = bName.BankName;
                        }
                    }
                }
                return merchentProfileModel;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the merchant profile.
        /// </summary>
        /// <param name="merchentProfileModel">The merchent profile model.</param>
        /// <param name="loginMerchantId">The login merchant identifier.</param>
        /// <returns></returns>
        public async Task<bool> UpdateMerchantProfile(MerchentProfileModel merchentProfileModel, long loginMerchantId)
        {
            try
            {
                var merchant = await _context.Merchant.Where(x => x.MerchantId == loginMerchantId && x.IsActive == true && !x.IsDeleted && !x.IsRejected).FirstOrDefaultAsync();
                if (merchant is not null)
                {
                    merchant.ProfilePic = merchentProfileModel.profileImage;
                    merchant.FirstName = merchentProfileModel.firstName;
                    merchant.LastName = merchentProfileModel.lastName;
                    merchant.FullName = merchentProfileModel.firstName + " " + merchentProfileModel.lastName;
                    merchant.EmailAddress = merchentProfileModel.emailAddress;
                    merchant.PhoneNumber = merchentProfileModel.phoneNumber;
                    merchant.UpdatedBy = loginMerchantId;
                    merchant.UpdatedOn = DateTime.UtcNow;

                    _context.Merchant.Update(merchant);
                    await _context.SaveChangesAsync();

                    var clinic = await _context.Clinic.Where(y => y.MerchantId == merchant.MerchantId && y.IsActive == true && !y.IsDeleted).FirstOrDefaultAsync();
                    if(clinic is not null)
                    {
                        clinic.ClinicName = merchentProfileModel.clinicName;
                        clinic.ClinicLocation = merchentProfileModel.clinicLocation;
                        clinic.ClinicImage = merchentProfileModel.Logo;
                        clinic.UpdatedBy = loginMerchantId;
                        clinic.UpdatedOn = DateTime.UtcNow;

                        _context.Clinic.Update(clinic);
                        await _context.SaveChangesAsync();

                        var merchantBank = await _context.MerchantBank.Where(i => i.MerchantId == merchant.MerchantId && !i.IsDeleted).FirstOrDefaultAsync();
                        if (merchantBank is not null)
                        {
                            merchantBank.BankName = merchentProfileModel.bankName;
                            merchantBank.BankLocation = merchentProfileModel.bankLocation;
                            merchantBank.AccountNumber = merchentProfileModel.accountNumber;
                            merchantBank.BSB = merchentProfileModel.bsbNumber;
                            merchantBank.UpdatedBy = loginMerchantId;
                            merchantBank.UpdatedOn = DateTime.UtcNow;

                            _context.MerchantBank.Update(merchantBank);
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

        #endregion
    }
}
