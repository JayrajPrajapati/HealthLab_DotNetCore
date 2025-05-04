using HealthLayby.Models.ApiViewModels.Home.Response;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// IHomeRepository
    /// </summary>
    public interface IHomeRepository
    {
        /// <summary>
        /// Gets the dashboard list.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        Task<(bool, string, List<HomeResponse>)> GetDashboardList(long customerId);
    }
}
