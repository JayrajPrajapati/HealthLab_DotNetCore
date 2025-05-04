using HealthLayby.Models.AdminViewModels;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// Dashboard controller
    /// </summary>
    /// <seealso cref="HealthLayby.Admin.Controllers.BaseController" />
    [Authorize]
    public class DashboardController : BaseController
    {
        #region Private Variables

        /// <summary>
        /// The customer repository
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// The merchant repository
        /// </summary>
        private readonly IMerchantRepository _merchantRepository;

        /// <summary>
        /// The service repository
        /// </summary>
        private readonly IServiceRepository _serviceRepository;

        /// <summary>
        /// The transaction history repository
        /// </summary>
        private readonly ITransactionHistoryRepository _transactionHistoryRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="customerRepository">The customer repository.</param>
        /// <param name="merchantRepository">The merchant repository.</param>
        /// <param name="serviceRepository">The service repository.</param>
        /// <param name="transactionHistoryRepository">The transaction history repository.</param>
        public DashboardController(IHttpContextAccessor httpContextAccessor,
                                   ICustomerRepository customerRepository,
                                   IMerchantRepository merchantRepository,
                                   IServiceRepository serviceRepository,
                                   ITransactionHistoryRepository transactionHistoryRepository) : base(httpContextAccessor)
        {
            _customerRepository = customerRepository;
            _merchantRepository = merchantRepository;
            _serviceRepository = serviceRepository;
            _transactionHistoryRepository = transactionHistoryRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                BindMessages();
                ViewBag.IsInnerContentDivRemove = true;
                var model = new DashboardModel
                {
                    CustomerCount = await _customerRepository.GetCustomerCountAsync(),
                    MerchantCount = await _merchantRepository.GetMerchantCountAsync(),
                    ServiceCount = await _serviceRepository.GetServiceCountAsync(),
                    CustomerChartData = await _customerRepository.GetCustomerChartDataAsync(),
                    MerchantChartData = await _merchantRepository.GetMerchantChartDataAsync()
                };

                return View(model);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the customer list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetCustomerList()
        {
            try
            {
                var result = await _customerRepository.TopCustomerGridListAsync();

                return Json(new
                {
                    draw = 1,
                    recordsTotal = result.Count,
                    recordsFiltered = result.Count,
                    data = result
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the merchant list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetMerchantList()
        {
            try
            {
                var result = await _merchantRepository.TopMerchantGridListAsync();

                return Json(new
                {
                    draw = 1,
                    recordsTotal = result.Count,
                    recordsFiltered = result.Count,
                    data = result
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the transaction list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetTransactionList()
        {
            try
            {
                var result = await _transactionHistoryRepository.TopTransactionGridListAsync();

                return Json(new
                {
                    draw = 1,
                    recordsTotal = result.Count,
                    recordsFiltered = result.Count,
                    data = result
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the plan history list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetPlanHistoryList()
        {
            try
            {
                var result = await _transactionHistoryRepository.TopPlanHistoryGridListAsync();

                return Json(new
                {
                    draw = 1,
                    recordsTotal = result.Count,
                    recordsFiltered = result.Count,
                    data = result
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
