using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels;
using HealthLayby.Models.ApiViewModels.Service.Request;
using HealthLayby.Repositories.Repositories;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.API.Controllers
{
    /// <summary>
    /// MerchantController
    /// </summary>
    /// <seealso cref="HealthLayby.API.Controllers.BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : BaseController
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
        /// The service repository
        /// </summary>
        private readonly IServiceRepository _serviceRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="serviceRepository">The service repository.</param>
        /// <param name="configuration">The configuration.</param>
        public ServiceController(IHttpContextAccessor httpContextAccessor, IServiceRepository serviceRepository, IConfiguration configuration) : base(httpContextAccessor)
        {
            _configuration = configuration;
            _serviceRepository = serviceRepository;
        }

        #endregion

        #region Public API's

        /// <summary>
        /// Services the list.
        /// </summary>
        /// <param name="sortBy">The sort by.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ServiceList")]
        public async Task<IActionResult> ServiceList(ServiceListingRequest serviceListingRequest)
        {
            try
            {
                var (isSuccess, message, ServiceListingResponse) = await _serviceRepository.GetServiceListForAPI(serviceListingRequest);

                if (isSuccess)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                                     (
                                         Status: StatusCodes.Status200OK,
                                         Message: message,
                                         Data: ServiceListingResponse
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

        [HttpPost]
        [Route("ServiceDetails")]
        public async Task<IActionResult> ServiceDetails(ServiceDetailsRequest serviceDetailsRequest)
        {
            try
            {
                var (isSuccess, message, ServiceDetailsResponse) = await _serviceRepository.GetServiceDetailsForAPI(claim.CustomerId, serviceDetailsRequest);
                if (isSuccess)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                                     (
                                         Status: StatusCodes.Status200OK,
                                         Message: message,
                                         Data: ServiceDetailsResponse
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
