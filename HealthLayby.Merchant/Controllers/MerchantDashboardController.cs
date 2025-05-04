using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Merchant.Controllers
{
    /// <summary>
    /// MerchantDashboardController
    /// </summary>
    /// <seealso cref="HealthLayby.Merchant.Controllers.BaseController" />
    [Authorize]
    public class MerchantDashboardController : BaseController
    {
        #region Private Variables

        private readonly IServiceRepository _serviceRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantDashboardController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public MerchantDashboardController(IServiceRepository serviceRepository, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _serviceRepository = serviceRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                BindMessages();
                var model = new DashboardMerchantModel
                {
                    Service = await _serviceRepository.GetServiceListForDashboard(claim.FirstName)
                };

                return View(model);
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
        //[HttpGet]
        //public async Task<IActionResult> GetTransactionList()
        //{
        //    BindMessages();
        //    List<TempLayByPlan> transactionList = await _serviceRepository.GetTransactionListForDashboard(claim.FirstName);

        //    return View(transactionList);
        //}

        #endregion
    }
}
