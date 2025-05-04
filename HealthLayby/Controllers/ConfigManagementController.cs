using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Admin.Controllers
{

    /// <summary>
    /// configmanagement controller
    /// </summary>
    /// <seealso cref="HealthLayby.Admin.Controllers.BaseController" />
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    public class ConfigManagementController : BaseController
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigManagementController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public ConfigManagementController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {

        }

        #endregion

        #region Public Methods

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
