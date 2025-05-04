using HealthLayby.Models.MerchentViewModels;

namespace HealthLayby.Repositories.Repositories.MerchantRepositories
{
    public interface IMerchantProfileWebRepository
    {
        /// <summary>
        /// Merchents the profile model.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        MerchentProfileModel GetMerchantProfile(long merchantId);

        /// <summary>
        /// Updates the merchant profile.
        /// </summary>
        /// <param name="merchentProfileModel">The merchent profile model.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<bool> UpdateMerchantProfile(MerchentProfileModel merchentProfileModel,long loginMerchantId);
    }
}
