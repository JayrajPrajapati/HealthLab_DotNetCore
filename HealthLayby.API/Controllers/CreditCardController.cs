using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels;
using HealthLayby.Models.ApiViewModels.Bank.Request;
using HealthLayby.Models.ApiViewModels.CreditCard.Request;
using HealthLayby.Repositories.Repositories;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.API.Controllers
{
    /// <summary>
    /// CreditCardController
    /// </summary>
    /// <seealso cref="HealthLayby.API.Controllers.BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : BaseController
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
        /// The credit card repository
        /// </summary>
        private readonly ICreditCardRepository _creditCardRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCardController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="creditCardRepository">The credit card repository.</param>
        /// <param name="configuration">The configuration.</param>
        public CreditCardController(IHttpContextAccessor httpContextAccessor, ICreditCardRepository creditCardRepository, IConfiguration configuration) : base(httpContextAccessor)
        {
            _configuration = configuration;
            _creditCardRepository = creditCardRepository;
        }

        #endregion

        #region Public API's

        [HttpPost]
        [Route("SaveUpdateCreditCard")]
        public async Task<IActionResult> SaveUpdateCreditCard([FromBody] SaveUpdateCreditCardRequest saveUpdateCreditCardRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.InvalidRequestPayload,
                        Data: ModelState
                    ));
                }

                var (isSuccess, message) = await _creditCardRepository.SaveUpdateCreditCardDetails(claim.CustomerId, saveUpdateCreditCardRequest);

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                                 (
                                     Status: StatusCodes.Status200OK,
                                     Message: message
                                 ));
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