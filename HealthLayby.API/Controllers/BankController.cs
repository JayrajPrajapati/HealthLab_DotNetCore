using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels;
using HealthLayby.Models.ApiViewModels.Bank.Request;
using HealthLayby.Repositories.Repositories;
using log4net;
using Microsoft.AspNetCore.Mvc;


namespace HealthLayby.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : BaseController
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
        /// The bank repository
        /// </summary>
        private readonly IBankRepository _bankRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BankController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="customerRepository">The customer repository.</param>
        /// <param name="configuration">The configuration.</param>
        public BankController(IHttpContextAccessor httpContextAccessor, IBankRepository bankRepository, IConfiguration configuration) : base(httpContextAccessor)
        {
            _configuration = configuration;
            _bankRepository = bankRepository;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        [Route("ValidateBSBNumber")]
        public async Task<IActionResult> ValidateBSBNumber([FromBody] ValidateBSBNumberRequest validateBSBNumberRequest)
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

                bool isValidateBSBNumber = await _bankRepository.IsValidateBSBNumberForAPI(validateBSBNumberRequest.BSBNumber);
                if (isValidateBSBNumber)
                {
                    var (isSuccess, message, ValidateBSBNumberResponse) = await _bankRepository.GetBankDetailAfterBSBValidationForAPI(validateBSBNumberRequest);

                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status200OK,
                        Message: ApiMessages.ValidBSBNumber,
                        Data: ValidateBSBNumberResponse
                    ));
                }
                else
                {
                    return StatusCode(StatusCodes.Status406NotAcceptable, new ResponseModel
                   (
                       Status: StatusCodes.Status406NotAcceptable,
                       Message: ApiMessages.InValidBSBNumber,
                       Data: ModelState
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

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }

        [HttpPost]
        [Route("SaveUpdateBankDetails")]
        public async Task<IActionResult> SaveUpdateBankDetails([FromBody] SaveBankDetailsRequest saveBankDetailsRequest)
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

                bool isValidateBSBNumber = await _bankRepository.IsValidateBSBNumberForAPI(saveBankDetailsRequest.BSBNumber);
                if (isValidateBSBNumber)
                {
                    var (isSuccess, message) = await _bankRepository.SaveBankDetails(claim.CustomerId,saveBankDetailsRequest);

                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status200OK,
                        Message: ApiMessages.SuccessfullBank                        
                    ));
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
                   (
                       Status: StatusCodes.Status400BadRequest,
                       Message: ApiMessages.InvalidBank,
                       Data: ModelState
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
