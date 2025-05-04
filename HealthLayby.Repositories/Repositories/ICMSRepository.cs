using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.ApiViewModels.ContentManagement.Response;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// ICMSRepository
    /// </summary>
    public interface ICMSRepository
    {

        /// <summary>
        /// Saves the content asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveContentAsync(CMSModel model, long adminId);

        /// <summary>
        /// Saves the FAQ asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveFAQAsync(FAQModel model, long adminId);

        /// <summary>
        /// Gets the FAQ by identifier asynchronous.
        /// </summary>
        /// <param name="faqId">The FAQ identifier.</param>
        /// <returns></returns>
        Task<FAQModel?> GetFAQByIdAsync(long faqId);

        /// <summary>
        /// Deletes the FAQ asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="rejectReason">The reject reason.</param>
        /// <returns></returns>
        Task<Tuple<bool, string>> DeleteFAQAsync(long id, long adminId, string rejectReason);

        /// <summary>
        /// Gets the FAQ list asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<FAQModel>> GetFAQListAsync();

        /// <summary>
        /// Gets the page content by page code asynchronous.
        /// </summary>
        /// <param name="pageCode">The page code.</param>
        /// <returns></returns>
        Task<CMSModel?> GetPageContentByPageCodeAsync(int pageCode);

        /// <summary>
        /// Gets the termsand condition content by page code asynchronous.
        /// </summary>
        /// <param name="pageCode">The page code.</param>
        /// <returns></returns>
        Task<CMSModel?> GetTermsandConditionContentByPageCodeAsync(int pageCode);

        /// <summary>
        /// Gets the why including admin fee content by page code asynchronous.
        /// </summary>
        /// <param name="pageCode">The page code.</param>
        /// <returns></returns>
        Task<CMSModel?> GetWhyIncludingAdminFeeContentByPageCodeAsync(int pageCode);

        /// <summary>
        /// Gets the content by page code asynchronous for API.
        /// </summary>
        /// <param name="contentManagementEnum">The content management enum.</param>
        /// <returns></returns>
        Task<ContentResponse?> GetContentByPageCodeAsyncForAPI(ContentManagementEnum contentManagementEnum);

        /// <summary>
        /// Gets the FAQ list asynchronous for API.
        /// </summary>
        /// <returns></returns>
        Task<List<FAQResponse>> GetFAQListAsyncForAPI();
    }
}
