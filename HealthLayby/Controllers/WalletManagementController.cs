using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// walletManagementController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class WalletManagementController : Controller
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WalletManagementController" /> class.
        /// </summary>
        public WalletManagementController()
        {

        }

        #endregion

        #region Public Method

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
