using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.Models;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// merchantRequestRepository
    /// </summary>
    public interface IMerchantRequestRepository
    {
        /// <summary>
        /// Gets the merchant request list.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        Task<Tuple<List<MerchantRequestGridListResult>, int, int>> GetMerchantRequestListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText);

        /// <summary>
        /// Gets the merchant request by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<MerchantModel?> GetMerchantRequestByIdAsync(long id);

        /// <summary>
        /// Gets the merchant request by identifier for reject request asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<MerchantRequestModel?> GetMerchantRequestByIdForRejectRequestAsync(long id);

        /// <summary>
        /// Rejects the merchant request asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="merchantRequestenum">The merchant requestenum.</param>
        /// <param name="rejectedReason">The rejected reason.</param>
        /// <param name="categoryid">The categoryid.</param>
        /// <param name="serviceIds">The service ids.</param>
        /// <returns></returns>
        Task<(bool, string)> AcceptAndRejectMerchantRequestAsync(long id, long adminId, MerchantRequestEnum merchantRequestenum, string rejectedReason, long categoryid =0, string serviceIds= "");

        /// <summary>
        /// Gets the merchant full name and email by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<MerchantModel?> GetMerchantFullNameAndEmailByIdAsync(long id);
    }
}
