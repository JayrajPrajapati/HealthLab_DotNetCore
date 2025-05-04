using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.MerchentViewModels;

namespace HealthLayby.Repositories.Repositories.MerchantRepositories
{
    /// <summary>
    /// IMerchantTermsWebRepository
    /// </summary>
    public interface IMerchantTermsWebRepository
    {
        /// <summary>
        /// Gets the merchant terms.
        /// </summary>
        /// <returns></returns>
        Task<MerchantTermsConditionModel> GetMerchantTerms();

        /// <summary>
        /// Saves the terms condition asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveTermsConditionAsync(TermsConditionModel model, long merchantId);
    }
}
