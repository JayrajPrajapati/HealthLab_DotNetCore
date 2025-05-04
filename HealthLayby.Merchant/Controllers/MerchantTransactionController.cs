using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Merchant.Controllers
{
    /// <summary>
    /// MerchantTransactionController
    /// </summary>
    /// <seealso cref="HealthLayby.Merchant.Controllers.BaseController" />
    public class MerchantTransactionController : BaseController
    {
        #region Private Variables

        /// <summary>
        /// The merchant earnings web repository
        /// </summary>
        private readonly IMerchantTransactionWebRepository _merchantTransactionWebRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantTransactionController" /> class.
        /// </summary>
        /// <param name="merchantTransactionWebRepository">The merchant transaction web repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public MerchantTransactionController(IMerchantTransactionWebRepository merchantTransactionWebRepository, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _merchantTransactionWebRepository = merchantTransactionWebRepository;
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
