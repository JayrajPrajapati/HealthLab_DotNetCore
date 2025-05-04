using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.Models;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// Lay By Repository
    /// </summary>
    public interface ILayByRepository
    {
        /// <summary>
        /// Gets the lay by list asynchronous.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        Task<Tuple<List<LayByPlanListResult>, int, int>> GetLayByListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText,long status);

        /// <summary>
        /// Gets the lay by by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<LayByModel?> GetLayByByIdAsync(long id);
    }
}
