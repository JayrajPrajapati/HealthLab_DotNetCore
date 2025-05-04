using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels;
using HealthLayby.Models.ApiViewModels.CustomerPlans.Request;
using HealthLayby.Repositories.Repositories;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.API.Controllers
{
    /// <summary>
    /// CustomerPlansController
    /// </summary>
    /// <seealso cref="HealthLayby.API.Controllers.BaseController" />
    [Route("api/[controller]")]
    [ApiController]

    public class CustomerPlansController : BaseController
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
        /// The customer plans repository
        /// </summary>
        private readonly ICustomerPlansRepository _customerPlansRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerPlansController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="customerPlansRepository">The customer plans repository.</param>
        /// <param name="configuration">The configuration.</param>
        public CustomerPlansController(IHttpContextAccessor httpContextAccessor, ICustomerPlansRepository customerPlansRepository, IConfiguration configuration) : base(httpContextAccessor)
        {
            _configuration = configuration;
            _customerPlansRepository = customerPlansRepository;
        }

        #endregion

        #region Public API's

        [HttpGet]
        [Authorize]
        [Route("LayByPlansList")]
        public async Task<IActionResult> LayByPlansList()
        {
            try
            {
                var (isSuccess, message, CustomerPlansResponse) = await _customerPlansRepository.GetPlanList(claim.CustomerId);

                if (isSuccess)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                                     (
                                         Status: StatusCodes.Status200OK,
                                         Message: message,
                                         Data: CustomerPlansResponse
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

        /// <summary>
        /// Lays the by plans view.
        /// </summary>
        /// <param name="planReviewRequestModel">The plan review request model.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("LayByPlansView")]
        public async Task<IActionResult> LayByPlansView(PlanViewRequestModel planReviewRequestModel)
        {

            try
            {
                var (isSuccess, message, PlansReviewResponse) = await _customerPlansRepository.GetPlanViewDetails(claim.CustomerId, planReviewRequestModel);
                if (isSuccess)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                                     (
                                         Status: StatusCodes.Status200OK,
                                         Message: message,
                                         Data: PlansReviewResponse
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

            #endregion
        }

        [HttpPost]
        [Authorize]
        [Route("GetAgreement")]
        public async Task<IActionResult> GetAgreement(AgreementRequest agreementRequest)
        {
            try
            {
                var (isSuccess, message, PlansReviewResponse) = await _customerPlansRepository.GetAgreement(claim.CustomerId, agreementRequest);
                if (isSuccess)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                                     (
                                         Status: StatusCodes.Status200OK,
                                         Message: message,
                                         Data: PlansReviewResponse
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
            catch (Exception ex )
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
    }
}
