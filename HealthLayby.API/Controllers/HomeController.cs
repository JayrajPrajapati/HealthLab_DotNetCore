using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels;
using HealthLayby.Repositories.Repositories;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.API.Controllers
{
    /// <summary>
    /// HomeController
    /// </summary>
    /// <seealso cref="HealthLayby.API.Controllers.BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : BaseController
    {
        #region Private members

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);
        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The home repository
        /// </summary>
        private readonly IHomeRepository _homeRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="homeRepository">The home repository.</param>
        /// <param name="configuration">The configuration.</param>
        public HomeController(IHttpContextAccessor httpContextAccessor, IHomeRepository homeRepository, IConfiguration configuration) : base(httpContextAccessor)
        {
            _configuration = configuration;
            _homeRepository = homeRepository;
        }

        #endregion

        #region Public API's


        [HttpGet]
        [Authorize]
        [Route("DashBoard")]
        public async Task<IActionResult> DashBoard()
        {
            try
            {
                var (isSuccess, message, HomeResponse) = await _homeRepository.GetDashboardList(claim.CustomerId);

                if (isSuccess)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                                     (
                                         Status: StatusCodes.Status200OK,
                                         Message: message,
                                         Data: HomeResponse
                                     ));
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
                                     (
                                         Status: StatusCodes.Status400BadRequest,
                                         Message: message
                                     ));
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.APILogError
                                    (
                                        logger: _logger,
                                        hostUrl: $"{Request.Scheme}://{Request.Host.Value}{Request.Path}",
                                        exception: ex,
                                        jwtToken: Request.Headers.TryGetValue("Authorization", out var token) ? token.ToString() : null,
                                        requestData: null
                                    );

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }
        #endregion
    }
}