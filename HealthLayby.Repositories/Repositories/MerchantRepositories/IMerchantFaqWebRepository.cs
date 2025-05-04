using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.MerchentViewModels;

namespace HealthLayby.Repositories.Repositories.MerchantRepositories
{
    /// <summary>
    /// IMerchantFaqWebRepository
    /// </summary>
    public interface IMerchantFaqWebRepository
    {
        /// <summary>
        /// Gets the merchant FAQ.
        /// </summary>
        /// <returns></returns>
        Task<List<FAQMerchantModel>> GetMerchantFaq();

        /// <summary>
        ///   Gets the FAQ by identifier asynchronous.
        /// </summary>
        /// <param name="faqId">The FAQ identifier.</param>
        /// <returns></returns>
        Task<FAQMerchantModel?> GetFAQByIdAsync(long faqId);

        /// <summary>
        ///   Saves the FAQ asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveFAQAsync(FAQMerchantModel model, long merchantId);

        /// <summary>
        /// Deletes the FAQ asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="rejectReason">The reject reason.</param>
        /// <returns></returns>
        Task<Tuple<bool, string>> DeleteFAQAsync(long id, long merchantId, string rejectReason);
    }
}
