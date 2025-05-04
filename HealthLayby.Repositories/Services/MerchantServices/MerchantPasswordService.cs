using HealthLayby.Models.Context;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Repositories.Services.MerchantServices
{
    /// <summary>
    /// MerchantPasswordService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.MerchantRepositories.IMerchantPasswordWebRepository" />
    public class MerchantPasswordService : BaseService, IMerchantPasswordWebRepository
    {
        #region Private Variable 
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantProfileService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MerchantPasswordService(AppDbContext context) : base(context)
        {

        }
        #endregion

        #region Public Methods
        public async Task<MerchantChangePassword> GetMerchantOldPassword(long loginMerchantId)
        {
            try
            {
                var merchant = await _context.Merchant.Where(x => x.MerchantId == loginMerchantId && x.IsActive == true && !x.IsDeleted && !x.IsRejected).FirstOrDefaultAsync();

                var model = new MerchantChangePassword()
                {
                    Password = merchant.Password
                };
                return model;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="loginMerchantId">The login merchant identifier.</param>
        /// <param name="merchantChangePassword">The merchant change password.</param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(long loginMerchantId, MerchantChangePassword merchantChangePassword)
        {
            var password = await _context.Merchant.Where(x => x.MerchantId == loginMerchantId && x.Password == merchantChangePassword.Password && x.IsActive == true && !x.IsDeleted).FirstOrDefaultAsync();
            if (password is not null) {
                password.Password = merchantChangePassword.NewPassword;
                password.UpdatedBy = loginMerchantId;
                password.UpdatedOn = DateTime.UtcNow;

                _context.Merchant.Update(password);
                await _context.SaveChangesAsync();

                return true;
            }
            else
                return false;
        }
    #endregion
}
}
