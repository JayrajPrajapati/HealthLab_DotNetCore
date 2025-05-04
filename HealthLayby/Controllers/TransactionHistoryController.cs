using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// transaction History Controller
    /// </summary>
    /// <seealso cref="HealthLayby.Admin.Controllers.BaseController" />
    public class TransactionHistoryController : BaseController
    {
        #region Private Variables

        /// <summary>
        /// The transaction history repository
        /// </summary>
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionHistoryController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="transactionHistoryRepository">The transaction history repository.</param>
        public TransactionHistoryController(IHttpContextAccessor httpContextAccessor,
                                                ITransactionHistoryRepository transactionHistoryRepository
                                            ) : base(httpContextAccessor)
        {
            _transactionHistoryRepository = transactionHistoryRepository;
        }

        #endregion

        #region public method

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the lay by transaction list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetLayByTransactionList()
        {
            try
            {
                Request.Form.TryGetValue("draw", out StringValues draw);
                Request.Form.TryGetValue("order[0][column]", out StringValues orderColumn);
                Request.Form.TryGetValue("order[0][dir]", out StringValues orderDirection);
                Request.Form.TryGetValue("start", out StringValues skipRecord);
                Request.Form.TryGetValue("length", out StringValues pageSize);
                Request.Form.TryGetValue("search[value]", out StringValues searchText);

                string sortingColumnName = orderColumn.ToString() switch
                {
                    "0" => "Transaction ID",
                    "1" => "Date",
                    "2" => "Customer",
                    "3" => "Paid Amount",
                    "4" => "Layby Plan",
                    _ => "Transaction ID",
                };

                var (data, totalRecord, totalFilteredRecord) = await _transactionHistoryRepository.GetTempLayByTransactionList
                (
                    sortColumn: sortingColumnName,
                    sortOrder: orderDirection.ToString(),
                    pageSize: Convert.ToInt32(pageSize),
                    pageIndex: Convert.ToInt32(skipRecord),
                    searchText: searchText
                );

                return Json(new
                {
                    draw = Convert.ToInt32(draw),
                    tranCount = totalRecord,
                    recordsTotal = totalFilteredRecord,
                    recordsFiltered = totalFilteredRecord,
                    data
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the wallet transaction list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetWalletTransactionList()
        {
            try
            {
                Request.Form.TryGetValue("draw", out StringValues draw);
                Request.Form.TryGetValue("order[0][column]", out StringValues orderColumn);
                Request.Form.TryGetValue("order[0][dir]", out StringValues orderDirection);
                Request.Form.TryGetValue("start", out StringValues skipRecord);
                Request.Form.TryGetValue("length", out StringValues pageSize);
                Request.Form.TryGetValue("search[value]", out StringValues searchText);

                string sortingColumnName = orderColumn.ToString() switch
                {
                    "0" => "Transaction ID",
                    "1" => "Date",
                    "2" => "Customer",
                    "3" => "Paid Amount",
                    "4" => "Commission",
                    _ => "Transaction ID",
                };

                var (data, totalRecord, totalFilteredRecord) = await _transactionHistoryRepository.GetWalletTransactionList
                (
                    sortColumn: sortingColumnName,
                    sortOrder: orderDirection.ToString(),
                    pageSize: Convert.ToInt32(pageSize),
                    pageIndex: Convert.ToInt32(skipRecord),
                    searchText: searchText
                );

                return Json(new
                {
                    draw = Convert.ToInt32(draw),
                    walletTranCount = totalRecord,
                    recordsTotal = totalFilteredRecord,
                    recordsFiltered = totalFilteredRecord,
                    data
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the direct pay transaction list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetDirectPayTransactionList()
        {
            try
            {
                Request.Form.TryGetValue("draw", out StringValues draw);
                Request.Form.TryGetValue("order[0][column]", out StringValues orderColumn);
                Request.Form.TryGetValue("order[0][dir]", out StringValues orderDirection);
                Request.Form.TryGetValue("start", out StringValues skipRecord);
                Request.Form.TryGetValue("length", out StringValues pageSize);
                Request.Form.TryGetValue("search[value]", out StringValues searchText);

                string sortingColumnName = orderColumn.ToString() switch
                {
                    "0" => "Transaction ID",
                    "1" => "Date",
                    "2" => "Customer",
                    "3" => "Paid Amount",
                    "4" => "Me",
                    _ => "Transaction ID",
                };

                var (data, totalRecord, totalFilteredRecord) = await _transactionHistoryRepository.GetDirectPayTransactionList
                (
                    sortColumn: sortingColumnName,
                    sortOrder: orderDirection.ToString(),
                    pageSize: Convert.ToInt32(pageSize),
                    pageIndex: Convert.ToInt32(skipRecord),
                    searchText: searchText
                );

                return Json(new
                {
                    draw = Convert.ToInt32(draw),
                    directPayTranCount = totalRecord,
                    recordsTotal = totalFilteredRecord,
                    recordsFiltered = totalFilteredRecord,
                    data
                });
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
