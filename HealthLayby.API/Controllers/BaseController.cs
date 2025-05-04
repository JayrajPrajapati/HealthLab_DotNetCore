using HealthLayby.API.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.API.Controllers
{
    /// <summary>
    /// BaseController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        #region Protected Fields

        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        protected readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// The claim
        /// </summary>
        protected readonly ClaimModel claim;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            claim = new ClaimModel(httpContextAccessor);
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion
    }
}
