using HealthLayby.API.Infrastructure;
using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels;
using HealthLayby.Models.ApiViewModels.Auth.Response;
using HealthLayby.Models.ApiViewModels.Registration.Request;
using HealthLayby.Repositories.Repositories;
using log4net;
using Microsoft.AspNetCore.Mvc;
using ApiCustomerModel = HealthLayby.Models.ApiViewModels.Common.CustomerModel;


namespace HealthLayby.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
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
        /// The customer repository
        /// </summary>
        private readonly ICustomerRepository _customerRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="customerRepository">The customer repository.</param>
        /// <param name="configuration">The configuration.</param>
        public RegistrationController(IHttpContextAccessor httpContextAccessor, ICustomerRepository customerRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _customerRepository = customerRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Signs up.
        /// </summary>
        /// <param name="signUpRequest">The sign up request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequest signUpRequest)
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

                if (signUpRequest.Password != signUpRequest.ConfirmPassword)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.NewPasswordAndVerifyPasswordDoesNotMatch,
                        Data: ModelState
                    ));
                }

                bool IsEmailAddressCheck = await _customerRepository.IsCustomerEmailAvailableAsyncForAPI(signUpRequest.EmailAddress);
                if (IsEmailAddressCheck)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
                    (
                        Status: StatusCodes.Status406NotAcceptable,
                        Message: ApiMessages.AlreadyAddedEmail,
                        Data: ModelState
                    ));
                }


                var (isSuccess, message, SignUpResponseResult) = await _customerRepository.SaveCustomerAsyncForAPI(signUpRequest);

                if (SignUpResponseResult is null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.InvalidRegistration,
                        Data: ModelState
                    ));
                }
                else
                {
                    ApiCustomerModel apiCustomerModel = new ApiCustomerModel
                    {
                        CustomerId = SignUpResponseResult.CustomerId,
                        FirstName = SignUpResponseResult.FirstName,
                        LastName = SignUpResponseResult.LastName,
                        FullName = SignUpResponseResult.FullName,
                        EmailAddress = SignUpResponseResult.EmailAddress,
                        PhoneNumber = "",
                        Password = SignUpResponseResult.Password,
                    };

                    string strToken = ApiCommonLogic.GenerateJWTToken(apiCustomerModel, _configuration);
                    

                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status200OK,
                        Message: ApiMessages.SuccessRegistration,
                        //Data: SignUpResponseResult
                        Data: new LoginResponse
                        {
                            CustomerId = SignUpResponseResult.CustomerId,
                            FirstName = SignUpResponseResult.FirstName,
                            LastName = SignUpResponseResult.LastName,
                            EmailAddress = SignUpResponseResult.EmailAddress,
                            //Password = SignUpResponseResult.Password,
                            PhoneNumber = "",
                            FullName = SignUpResponseResult.FullName,
                            AuthToken = string.IsNullOrEmpty(strToken) ? "" : strToken,
                            ProfilePic = ""
                        }
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

        #endregion
    }
}
