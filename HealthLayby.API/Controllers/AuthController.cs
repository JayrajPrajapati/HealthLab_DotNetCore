using HealthLayby.API.Infrastructure;
using HealthLayby.API.Infrastructure.EmailHelper;
using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels;
using HealthLayby.Models.ApiViewModels.Auth.Request;
using HealthLayby.Models.ApiViewModels.Auth.Response;
using HealthLayby.Repositories.Repositories;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiCustomerModel = HealthLayby.Models.ApiViewModels.Common.CustomerModel;

namespace HealthLayby.API.Controllers
{
    /// <summary>
    /// AuthController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
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

        /// <summary>
        /// The env
        /// </summary>
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// The i customer forgot password token repository
        /// </summary>
        private readonly ICustomerForgotPasswordTokenRepository _iCustomerForgotPasswordTokenRepository;

        /// <summary>
        ///   The email service
        /// </summary>
        private readonly IEmailService _emailService;


        /// <summary>
        /// The string customer profile root path
        /// </summary>
        private readonly string _strCustomerProfileRootPath;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="customerRepository">The customer repository.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="env">The env.</param>
        /// <param name="CustomerForgotPasswordTokenRepository">The customer forgot password token repository.</param>
        public AuthController(IHttpContextAccessor httpContextAccessor,
                              ICustomerRepository customerRepository,
                              IConfiguration configuration,
                              IWebHostEnvironment env,
                              ICustomerForgotPasswordTokenRepository CustomerForgotPasswordTokenRepository,
                              IEmailService emailService) : base(httpContextAccessor)
        {
            _configuration = configuration;
            _customerRepository = customerRepository;
            _env = env;
            _iCustomerForgotPasswordTokenRepository = CustomerForgotPasswordTokenRepository;
            _emailService = emailService;

            _strCustomerProfileRootPath = _configuration.GetValue("Urls:AdminPortalURL", "");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Logins the specified login request.
        /// </summary>
        /// <param name="loginRequest">The login request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
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

                var data = await _customerRepository.GetCustomerByEmailAddressAsync(loginRequest.EmailAddress);

                if (data is null || CommonLogic.Encrypt(loginRequest.Password) != data.Password)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.IncorrectEmailPassword,
                        Data: ModelState
                    ));
                }

                string strToken = ApiCommonLogic.GenerateJWTToken(data, _configuration);

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status200OK,
                    Message: ApiMessages.LoginSuccessfully,
                    Data: new LoginResponse
                    {
                        CustomerId = data.CustomerId,
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        EmailAddress = data.EmailAddress,
                        //Password = data.Password,
                        PhoneNumber = data.PhoneNumber,
                        FullName = data.FullName,
                        AuthToken = strToken,
                        ProfilePic = !string.IsNullOrWhiteSpace(data.ProfilePic) ? $"{_strCustomerProfileRootPath}{DirectoryConstant.CustomerProfilePicDirectory}/{data.ProfilePic}" : data.ProfilePic
                    }
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
                    requestData: JsonConvert.SerializeObject(loginRequest)
                );

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }

        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <param name="forgetPasswordRequest">The forget password request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPasswordRequest forgetPasswordRequest)
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

                var data = await _customerRepository.GetCustomerByEmailAddressAsync(forgetPasswordRequest.EmailAddress);

                if (data is null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.NoAccountExistsUsingEmail,
                        Data: ModelState
                    ));
                }

                string strOTP = GenerateRandomOTPToVerifyEmail();
                var token = Guid.NewGuid();
                var unusedToken = await _iCustomerForgotPasswordTokenRepository.GetUnusedTokenForCustomerAsync(data.CustomerId);

                if (unusedToken is null || unusedToken == Guid.Empty)
                {
                    await _iCustomerForgotPasswordTokenRepository.AddForgotPasswordTokenForCustomerAsync(data.CustomerId, token);
                }
                else
                {
                    token = unusedToken.Value;
                }

                var html = GetForgetPasswordHTML
                (
                   html: System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "templates/ForgotPassword.html")),
                   userName: $"{data.FullName}",
                   strOTP: strOTP
                );

                await _emailService.SendEmail
                (
                    toEmail: new string[] { data.EmailAddress },
                    subject: $"Password reset for - {data.FullName}",
                    mailBody: html
                );

                string strEncyptedKey = CommonLogic.Encrypt($"{token.ToString()}:{strOTP}");

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status200OK,
                    Message: ApiMessages.CheckInboxForOtp,
                    Data: new ForgetPasswordResponse
                    {
                        Key = strEncyptedKey
                    }
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
                    requestData: JsonConvert.SerializeObject(forgetPasswordRequest)
                );

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }

        /// <summary>
        /// Forgets the password verify otp for.
        /// </summary>
        /// <param name="forgetPasswordVerifyOtpRequest">The forget password verify otp request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("VerifyOTP")]
        public async Task<IActionResult> ForgetPasswordVerifyOtpFor([FromBody] ForgetPasswordVerifyOtpRequest forgetPasswordVerifyOtpRequest)
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

                var keyArray = CommonLogic.Decrypt(forgetPasswordVerifyOtpRequest.Key).Split(':');
                string strGuID = string.Empty, strOTP = string.Empty;
                if (keyArray != null)
                {
                    strGuID = keyArray[0];
                    strOTP = keyArray[1];
                }

                if (string.IsNullOrWhiteSpace(strGuID) || string.IsNullOrWhiteSpace(strOTP))
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.InvalidRequestPayload,
                        Data: ModelState
                    ));
                }

                var CustomerId = await _iCustomerForgotPasswordTokenRepository.GetCustomerIdByTokenAsync(new Guid(strGuID));

                if (CustomerId is null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.InvalidRequestPayload,
                        Data: ModelState
                    ));
                }

                if (strOTP != forgetPasswordVerifyOtpRequest.OTP)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.ProvidedOTPDoesNotMatch,
                        Data: ModelState
                    ));
                }

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status200OK,
                    Message: ApiMessages.OTPVerifiedSuccessfully,
                    Data: new ForgetPasswordResponse
                    {
                        Key = forgetPasswordVerifyOtpRequest.Key
                    }
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
                    requestData: JsonConvert.SerializeObject(forgetPasswordVerifyOtpRequest)
                );

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPasswordRequest">The reset password request.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
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

                if (resetPasswordRequest.Password != resetPasswordRequest.ConfirmPassword)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.NewPasswordAndVerifyPasswordDoesNotMatch,
                        Data: ModelState
                    ));
                }

                var keyArray = CommonLogic.Decrypt(resetPasswordRequest.Key).Split(':');
                string strGuID = string.Empty, strOTP = string.Empty;
                if (keyArray != null)
                {
                    strGuID = keyArray[0];
                    strOTP = keyArray[1];
                }

                if (string.IsNullOrWhiteSpace(strGuID) || string.IsNullOrWhiteSpace(strOTP))
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.InvalidRequestPayload,
                        Data: ModelState
                    ));
                }

                var CustomerId = await _iCustomerForgotPasswordTokenRepository.GetCustomerIdByTokenAsync(new Guid(strGuID));
                if (CustomerId is null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.InvalidRequestPayload,
                        Data: ModelState
                    ));
                }

                var customerData = await _customerRepository.GetCustomerByCustomerIdAsync(CustomerId.Value);
                if (customerData is null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.InvalidRequestPayload,
                        Data: ModelState
                    ));
                }

                await _customerRepository.UpdateCustomerPasswordAsync(customerData.CustomerId, CommonLogic.Encrypt(resetPasswordRequest.Password));

                await _iCustomerForgotPasswordTokenRepository.UpdateChangeDateAsync(new Guid(strGuID));

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status200OK,
                    Message: ApiMessages.PasswordUpdatedSuccessfully,
                    Data: ModelState
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
                    requestData: JsonConvert.SerializeObject(resetPasswordRequest)
                );

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="changePasswordRequest">The change password request.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePasswordRequest)
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

                if (changePasswordRequest.NewPassword != changePasswordRequest.ConfirmPassword)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.NewPasswordAndVerifyPasswordDoesNotMatch,
                        Data: ModelState
                    ));
                }

                if (changePasswordRequest.ExistingPassword == changePasswordRequest.NewPassword)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.NewPasswordAndExistingPassMustbeDifferent,
                        Data: ModelState
                    ));
                }

                var data = await _customerRepository.GetCustomerByCustomerIdAsync(claim.CustomerId);

                if (data == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.InvalidRequestPayload,
                        Data: ModelState
                    ));
                }

                if (data.Password != CommonLogic.Encrypt(changePasswordRequest.ExistingPassword))
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.ExistingPasswordDoesNotMatch,
                        Data: ModelState
                    ));
                }

                var dataUpdatePassword = await _customerRepository.UpdateCustomerPasswordAsync(claim.CustomerId, CommonLogic.Encrypt(changePasswordRequest.NewPassword));

                if (dataUpdatePassword)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status200OK,
                        Message: ApiMessages.PasswordUpdatedSuccessfully
                    ));
                }
                else
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.PasswordDoesNotUpdated,
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
                    requestData: JsonConvert.SerializeObject(changePasswordRequest)
                );

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }

        /// <summary>
        /// Generates the JWT token.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [NonAction]
        public string GenerateJWTToken(ApiCustomerModel data)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                     new Claim("CustomerId", data.CustomerId.ToString()),
                     //new Claim("EmailAddress", data.EmailAddress),
                     //new Claim("FirstName", data.FirstName),
                     //new Claim("LastName", data.LastName),
                     //new Claim("FullName", data.FullName),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                //Expires = DateTime.UtcNow.AddMinutes(5),
                Expires = DateTime.UtcNow.AddMonths(6),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials
                (
                    key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    algorithm: SecurityAlgorithms.HmacSha512Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the forget password HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="strOTP">The string otp.</param>
        /// <returns></returns>
        [NonAction]
        private string GetForgetPasswordHTML(string html, string userName, string strOTP)
        {
            try
            {
                var domain = $"{Request.Scheme}://{Request.Host}";
                html = html.Replace("##FULLNAME##", userName);
                html = html.Replace("##DOMAIN##", domain);
                return html.Replace("##VerifyOtp##", $"{strOTP}");
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Generates the random otp to verify email.
        /// </summary>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        [NonAction]
        private string GenerateRandomOTPToVerifyEmail(int length = 4)
        {
            try
            {
                int MinValue = (int)Math.Pow(10, length - 1);
                int MaxValue = (int)Math.Pow(10, length) - 1;

                Random rnd = new Random();
                long randomNumber = rnd.Next(MinValue, MaxValue);

                randomNumber = 4748;

                return randomNumber.ToString();
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
