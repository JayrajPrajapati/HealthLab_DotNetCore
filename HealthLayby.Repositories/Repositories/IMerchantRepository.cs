using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.ApiViewModels.Merchant.Request;
using HealthLayby.Models.ApiViewModels.Merchant.Response;
using HealthLayby.Models.Models;

namespace HealthLayby.Repositories.Repositories
{

    /// <summary>
    /// merchantRepository
    /// </summary>
    public interface IMerchantRepository
    {

        /// <summary>
        /// Gets the merchant list.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        Task<Tuple<List<MerchantGridListResult>, int, int>> GetMerchantListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText);

        /// <summary>
        /// Gets the merchant by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<MerchantModel?> GetMerchantByIdAsync(long id);

        /// <summary>
        /// Saves the merchant asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveMerchantAsync(MerchantModel model, long adminId, string? password = null);

        /// <summary>
        /// Deletes the merchant asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<Tuple<bool, string>> DeleteMerchantAsync(long id, long adminId);

        /// <summary>
        /// Determines whether [is merchant email exist asynchronous] [the specified merchantid].
        /// </summary>
        /// <param name="merchantid">The merchantid.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Task<bool> IsMerchantEmailExistAsync(long? merchantid, string email);

        /// <summary>
        /// Tops the merchant grid list asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<TopMerchantGridListResult>> TopMerchantGridListAsync();

        /// <summary>
        /// Gets the merchant total count asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<int> GetMerchantCountAsync();

        /// <summary>
        /// Gets the merchant.
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, int>> GetMerchantChartDataAsync();

        #region For API

        /// <summary>
        /// Gets the merchant list.
        /// </summary>
        /// <param name="merchentListingRequest">The merchent listing request.</param>
        /// <returns></returns>
        Task<(bool, string, List<MerchantListingResponse>)> GetMerchantListForAPI(MerchentListingRequest merchentListingRequest);

        #endregion

    }
}
