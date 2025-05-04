using HealthLayby.Models.Models;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// ITransactionHistoryRepository
    /// </summary>
    public interface ITransactionHistoryRepository
    {
        /// <summary>
        /// Gets the temporary lay by transaction list.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        Task<Tuple<List<TempLayByTransactionGridListResult>, int, int>> GetTempLayByTransactionList(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText);

        /// <summary>
        /// Gets the wallet transaction list.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        Task<Tuple<List<WalletTransactionGridListResult>, int, int>> GetWalletTransactionList(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText);

        /// <summary>
        /// Gets the direct pay transaction list.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        Task<Tuple<List<DirectPayTransactionGridListResult>, int, int>> GetDirectPayTransactionList(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText);

        /// <summary>
        /// Tops the transaction grid list asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<TopTransactionGridListResult>> TopTransactionGridListAsync();

        /// <summary>
        /// Tops the plan history grid list asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<TopPlanHistoryGridListResult>> TopPlanHistoryGridListAsync();
    }
}
