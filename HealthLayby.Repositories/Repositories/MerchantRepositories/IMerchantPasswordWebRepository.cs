using HealthLayby.Models.MerchentViewModels;

namespace HealthLayby.Repositories.Repositories.MerchantRepositories
{
    /// <summary>
    /// MerchantPasswordService
    /// </summary>
    public interface IMerchantPasswordWebRepository
    {
        /// <summary>
        /// Gets the merchant old password.
        /// </summary>
        /// <param name="loginMerchantId">The login merchant identifier.</param>
        /// <returns></returns>
        Task<MerchantChangePassword> GetMerchantOldPassword(long loginMerchantId);

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="loginMerchantId">The login merchant identifier.</param>
        /// <param name="merchantChangePassword">The merchant change password.</param>
        /// <returns></returns>
        Task<bool> ChangePassword(long loginMerchantId, MerchantChangePassword merchantChangePassword);
    }
}
