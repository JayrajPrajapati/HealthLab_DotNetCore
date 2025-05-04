using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Merchant.Controllers
{
    /// <summary>
    /// MerchantCustomer
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    public class MerchantCustomerController : BaseController
    {

        #region Private Variables

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantCustomerController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public MerchantCustomerController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        #endregion

        #region Public's Methods 

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
