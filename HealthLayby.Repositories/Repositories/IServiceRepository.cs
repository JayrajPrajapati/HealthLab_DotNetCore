using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.ApiViewModels.Merchant.Request;
using HealthLayby.Models.ApiViewModels.Merchant.Response;
using HealthLayby.Models.ApiViewModels.Service.Request;
using HealthLayby.Models.ApiViewModels.Service.Response;
using HealthLayby.Models.Models;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// service repository
    /// </summary>
    public interface IServiceRepository
    {
        /// <summary>
        /// Saves the service asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveServiceAsync(ServiceModel model, long adminId);

        /// <summary>
        /// Services the grid list asynchronous.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        Task<Tuple<List<ServiceGridListResult>, int, int>> ServiceGridListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText);

        /// <summary>
        /// Gets the service by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<ServiceModel?> GetServiceModelByIdAsync(long id);

        /// <summary>
        /// Deletes the service aysnc.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<Tuple<bool, string>> DeleteServiceAysnc(long id, long adminId);

        /// <summary>
        /// Determines whether [is service name available] [the specified service name].
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        Task<bool> IsServiceNameAvailableAsync(string serviceName, long serviceId, long categoryId);

        /// <summary>
        /// Gets the service total count.
        /// </summary>
        /// <returns></returns>
        Task<int> GetServiceCountAsync();


        #region Mobile API

        /// <summary>
        /// Gets the service list for API.
        /// </summary>
        /// <param name="serviceListingRequest">The service listing request.</param>
        /// <returns></returns>
        Task<(bool, string, List<ServiceListingResponse>)> GetServiceListForAPI(ServiceListingRequest serviceListingRequest);

        /// <summary>
        /// Gets the service details for API.
        /// </summary>
        /// <param name="CustomerId">The customer identifier.</param>
        /// <param name="serviceDetailsRequest">The service details request.</param>
        /// <returns></returns>
        Task<(bool, string, ServiceDetailsResponse)> GetServiceDetailsForAPI(long CustomerId, ServiceDetailsRequest serviceDetailsRequest);

        #endregion

        #region Merchent

        /// <summary>
        /// Gets the service list for dashboard.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns></returns>
        Task<List<Service>> GetServiceListForDashboard(string firstName);

        /// <summary>
        /// Gets the transaction list for dashboard.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns></returns>
        Task<List<TempLayByPlan>> GetTransactionListForDashboard(string firstName);

        #endregion
    }
}
