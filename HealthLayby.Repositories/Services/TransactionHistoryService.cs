using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;

namespace HealthLayby.Repositories.Services
{

    /// <summary>
    /// transactionHistoryService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.ITransactionHistoryRepository" />
    public class TransactionHistoryService : BaseService, ITransactionHistoryRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionHistoryService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TransactionHistoryService(AppDbContext context) : base(context)
        {

        }



        #endregion

        #region Public Method

        /// <summary>
        /// Gets the temporary lay by transaction list.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public async Task<Tuple<List<TempLayByTransactionGridListResult>, int, int>> GetTempLayByTransactionList(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText)
        {
            try
            {
                var paramTotalRecords = new OutputParameter<int?>();
                var paramTotalFilteredRecords = new OutputParameter<int?>();

                var result = await _context.GetProcedures().TempLayByTransactionGridListAsync
                (
                    SortColumn: sortColumn,
                    SortOrder: sortOrder,
                    PageSize: pageSize,
                    PageIndex: pageIndex,
                    SearchText: searchText,
                    TotalRecords: paramTotalRecords,
                    TotalFilteredRecords: paramTotalFilteredRecords
                );

                return Tuple.Create(result, paramTotalRecords.Value ?? 0, paramTotalFilteredRecords.Value ?? 0);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the wallet transaction list.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public async Task<Tuple<List<WalletTransactionGridListResult>, int, int>> GetWalletTransactionList(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText)
        {
            try
            {
                var paramTotalRecords = new OutputParameter<int?>();
                var paramTotalFilteredRecords = new OutputParameter<int?>();

                var result = await _context.GetProcedures().WalletTransactionGridListAsync
                (
                    SortColumn: sortColumn,
                    SortOrder: sortOrder,
                    PageSize: pageSize,
                    PageIndex: pageIndex,
                    SearchText: searchText,
                    TotalRecords: paramTotalRecords,
                    TotalFilteredRecords: paramTotalFilteredRecords
                );

                return Tuple.Create(result, paramTotalRecords.Value ?? 0, paramTotalFilteredRecords.Value ?? 0);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the direct pay transaction list.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public async Task<Tuple<List<DirectPayTransactionGridListResult>, int, int>> GetDirectPayTransactionList(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText)
        {
            try
            {
                var paramTotalRecords = new OutputParameter<int?>();
                var paramTotalFilteredRecords = new OutputParameter<int?>();

                var result = await _context.GetProcedures().DirectPayTransactionGridListAsync
                (
                    SortColumn: sortColumn,
                    SortOrder: sortOrder,
                    PageSize: pageSize,
                    PageIndex: pageIndex,
                    SearchText: searchText,
                    TotalRecords: paramTotalRecords,
                    TotalFilteredRecords: paramTotalFilteredRecords
                );

                return Tuple.Create(result, paramTotalRecords.Value ?? 0, paramTotalFilteredRecords.Value ?? 0);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Transactions the grid top record list asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TopTransactionGridListResult>> TopTransactionGridListAsync()
        {
            try
            {
                var result = await _context.GetProcedures().TopTransactionGridListAsync();

                return result;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Plans the history grid top record list asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TopPlanHistoryGridListResult>> TopPlanHistoryGridListAsync()
        {
            try
            {
                return await _context.GetProcedures().TopPlanHistoryGridListAsync();
            }
            catch
            {
                throw;
            }
        }

        #endregion

    }
}
