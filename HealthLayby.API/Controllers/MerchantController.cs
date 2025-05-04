using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels;
using HealthLayby.Models.ApiViewModels.Merchant.Request;
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
    public class MerchantController : BaseController
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
        /// The merchant repository
        /// </summary>
        private readonly IMerchantRepository _merchantRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="merchantRepository">The merchant repository.</param>
        /// <param name="configuration">The configuration.</param>
        public MerchantController(IHttpContextAccessor httpContextAccessor, IMerchantRepository merchantRepository, IConfiguration configuration) : base(httpContextAccessor)
        {
            _configuration = configuration;
            _merchantRepository = merchantRepository;
        }

        #endregion

        #region Public API's

        /// <summary>
        /// Merchents the list.
        /// </summary>
        /// <param name="merchentListingRequest">The merchent listing request.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("MerchentListing")]
        public async Task<IActionResult> MerchentList(MerchentListingRequest merchentListingRequest)
        {
            try
            {
                var (isSuccess, message, MerchantListingResponse) = await _merchantRepository.GetMerchantListForAPI(merchentListingRequest);

                if (isSuccess)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                                     (
                                         Status: StatusCodes.Status200OK,
                                         Message: message,
                                         Data: MerchantListingResponse
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
