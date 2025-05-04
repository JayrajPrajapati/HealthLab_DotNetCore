using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Merchant.Controllers
{
    /// <summary>
    /// MerchantEarningsController
    /// </summary>
    /// <seealso cref="HealthLayby.Merchant.Controllers.BaseController" />
    public class MerchantEarningsController : BaseController
    {
        #region Private Variables

        /// <summary>
        /// The merchant earnings web repository
        /// </summary>
        private readonly IMerchantEarningsWebRepository _merchantEarningsWebRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantEarningsController" /> class.
        /// </summary>
        /// <param name="merchantEarningsWebRepository">The merchant earnings web repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public MerchantEarningsController(IMerchantEarningsWebRepository merchantEarningsWebRepository, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _merchantEarningsWebRepository = merchantEarningsWebRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
