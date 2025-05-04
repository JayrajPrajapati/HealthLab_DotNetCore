using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels.Bank.Request;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json.Linq;
using Stripe;
using System.Dynamic;
using System.Net;
using System.Reflection.Emit;
using System.Security.Claims;

namespace HealthLayby.Merchant.Controllers
{
    /// <summary>
    /// LoginController
    /// </summary>
    /// <seealso cref="HealthLayby.Merchant.Controllers.BaseController" />
    public class LoginController : BaseController
    {
        #region Private Variable

        /// <summary>
        ///   The env
        /// </summary>
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// The merchant repository
        /// </summary>
        private readonly IMerchantWebRepository _merchantWebRepository;

        /// <summary>
        /// The bank repository
        /// </summary>
        private readonly IBankRepository _bankRepository;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="merchantWebRepository">The merchant web repository.</param>
        /// <param name="bankRepository">The bank repository.</param>
        /// <param name="env">The env.</param>
        /// <param name="configuration">The configuration.</param>
        public LoginController(IHttpContextAccessor httpContextAccessor, IMerchantWebRepository merchantWebRepository, IBankRepository bankRepository,IWebHostEnvironment env, IConfiguration configuration) : base(httpContextAccessor)
        {
            _merchantWebRepository = merchantWebRepository;
            _bankRepository = bankRepository;
            _env = env;
            _configuration = configuration;
        }

        #endregion

        #region Public Methods

        #region Login

        /// <summary>
        /// Indexes the specified return URL.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            try
            {
                BindMessages();

                if (returnUrl != null)
                {
                    ViewBag.ReturnUrl = returnUrl;
                }

                if (User.Identity?.IsAuthenticated ?? false)
                {
                    if (!string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return LocalRedirect(WebUtility.UrlDecode(returnUrl));
                    }
                }
                return View();
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginModel merchantModel, string? returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(merchantModel);
                }

                var data = await _merchantWebRepository.GetMerchantByEmailAddressAsync(merchantModel.EmailAddress);
                if (data is null)
                {
                    ModelState.AddModelError(nameof(merchantModel.Password), MessageConstant.InValidEmailAndPassword);
                    return View(merchantModel);
                }               

                if (CommonLogic.Encrypt(merchantModel.Password) != data.Password)
                {
                    ModelState.AddModelError(nameof(merchantModel.Password), MessageConstant.InValidPassword);
                    return View("Index", merchantModel);
                }

                if (data is not null && data.Status == 1)
                {
                    ModelState.AddModelError(nameof(merchantModel.Password), MessageConstant.BlockLogin);
                    return View(merchantModel);
                }

                var principal = new ClaimsPrincipal
                (
                    identity: new ClaimsIdentity
                    (
                        claims: new List<Claim>
                        {
                              new Claim("MerchantId", data.MerchantId.ToString()),
                              new Claim("EmailAddress", data.EmailAddress),
                              new Claim("FirstName", data.FirstName),
                              new Claim("LastName", data.LastName),
                              new Claim("FullName", data.FullName),
                        },
                        authenticationType: CookieAuthenticationDefaults.AuthenticationScheme
                    )
                );

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                    IsPersistent = true
                });

                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    return LocalRedirect(WebUtility.UrlDecode(returnUrl));
                }

                return RedirectToAction("Index", "MerchantDashboard");
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Forgot Password

        /// <summary>
        ///   Forgots the password.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            try
            {
                BindMessages();
                return View();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Forgots the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordMerchantModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.InvalidModalState;
                    return RedirectToRoute("forgotPassword");
                }

                var merchantData = await _merchantWebRepository.GetMerchantByEmailAddressAsync(model.EmailAddress);
                if (merchantData is null)
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.MerchantNotFound;
                    return RedirectToRoute("forgotPassword");
                }

                var token = Guid.NewGuid();
                var unusedToken = await _merchantWebRepository.GetUnusedTokenForMerchantAsync(merchantData.MerchantId);

                if (unusedToken is null || unusedToken == Guid.Empty)
                {
                    await _merchantWebRepository.AddForgotPasswordTokenForMerchantAsync(merchantData.MerchantId, token);
                }
                else
                {
                    token = unusedToken.Value;
                }

                var html = GetForgetPasswordHTML
                (
                   html: System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "templates/ForgotPassword.html")),
                   userName: $"{merchantData.FullName}",
                   passwordResetToken: token.ToString()
                );

                var mail = CommonLogic.SendEmail
                (
                   subject: $"Password reset for - {merchantData.FullName}",
                   body: html,
                   toEmail: merchantData.EmailAddress
                );

                if (string.IsNullOrWhiteSpace(mail))
                {
                    TempData[MessageConstant.SuccessMessageKey] = MessageConstant.ResetPasswordSuccess;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.ResetPasswordFailed;
                    return RedirectToRoute("forgotPassword");
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string? message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.InValidResetToken;
                    return RedirectToAction(nameof(Index));
                }

                if (message != null)
                {
                    TempData[MessageConstant.ErrorMessageKey] = message;
                    return RedirectToAction(nameof(Index));
                }

                if (!Guid.TryParse(token, out var _))
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.ExpiredResetToken;
                    return RedirectToAction(nameof(Index));
                }

                bool isTokenValid = await _merchantWebRepository.CheckMerchantTokenAsync(new Guid(token));

                if (!isTokenValid)
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.ExpiredResetToken;
                    return RedirectToAction(nameof(Index));
                }

                BindMessages();
                return View(new ResetPasswordMerchantModel { ResetToken = token });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="resetPasswordMerchantModel">The reset password merchant model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordMerchantModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[MessageConstant.ErrorMessageKey] = (model.Password != model.ConfirmPassword) ? MessageConstant.InValidPassword : MessageConstant.InvalidModalState;
                    return (model.Password != model.ConfirmPassword) ? RedirectToAction(nameof(ResetPassword), new { token = model.ResetToken, message = MessageConstant.InValidPassword }) : RedirectToRoute("forgotPassword");
                }

                var merchantId = await _merchantWebRepository.GetMerchantIdByTokenAsync(new Guid(model.ResetToken));
                if (merchantId.HasValue)
                {
                    var merchantData = await _merchantWebRepository.GetMerchantByIdAsync(merchantId.Value);
                    if (merchantData is null)
                    {
                        TempData[MessageConstant.ErrorMessageKey] = MessageConstant.ExpiredResetToken;
                        return RedirectToAction(nameof(ResetPassword), new { token = model.ResetToken });
                    }

                    await _merchantWebRepository.UpdateMerchantResetPasswordAsync(merchantData.MerchantId, CommonLogic.Encrypt(model.Password));

                    await _merchantWebRepository.UpdateMerchantChangeDateAsync(new Guid(model.ResetToken));

                    TempData[MessageConstant.SuccessMessageKey] = MessageConstant.ResetUpdatePasswordSuccess;
                    return RedirectToAction("Index", "Login");
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                throw;
            }
        }

        #endregion 

        #region Register

        /// <summary>
        /// Registers this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            try
            {
                BindMessages();
                return View();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterMerchantModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var isMerchantExists = await _merchantWebRepository.IsMerchantExists(model.Email, model.FirstName);
                if (isMerchantExists > 0)
                {
                    var token = Guid.NewGuid();
                    var unusedToken = await _merchantWebRepository.GetUnusedTokenForMerchantAsync(isMerchantExists);

                    if (unusedToken is null || unusedToken == Guid.Empty)
                    {
                        await _merchantWebRepository.AddForgotPasswordTokenForMerchantAsync(isMerchantExists, token);
                    }
                    else
                    {
                        token = unusedToken.Value;
                    }

                    Random generator = new Random();
                    String randomOTP = generator.Next(0, 1000000).ToString("D6");
                    var saveOTP = await _merchantWebRepository.AddGeneratedOTP(randomOTP, token);

                    var html = GetRegistrationHTML
                    (
                       html: System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "templates/RegistrationOtp.html")),
                       userName: $"{model.FirstName}",
                       otp: randomOTP,
                       email: model.Email
                    );

                    var mail = CommonLogic.SendEmail
                    (
                       subject: $"Password reset for - {model.FirstName}",
                       body: html,
                       toEmail: model.Email
                    );

                    OTPModel oTPModel = new OTPModel()
                    {
                        MerchantId = isMerchantExists,
                        EmailAddress = model.Email,
                        token = token
                    };

                    TempData[MessageConstant.SuccessMessageKey] = MessageConstant.RegisterEmailSuccess;
                    return RedirectToAction("SubmitOTP", oTPModel);
                }
                else
                {
                    var password = Password.Generate(10, 4);
                    var (isSuccess, message, merchantData) = await _merchantWebRepository.SaveMerchantAsync(model, claim.AdminId, password);

                    if (isSuccess)
                    {
                        var token = Guid.NewGuid();
                        var unusedToken = await _merchantWebRepository.GetUnusedTokenForMerchantAsync(merchantData.MerchantId);

                        if (unusedToken is null || unusedToken == Guid.Empty)
                        {
                            await _merchantWebRepository.AddForgotPasswordTokenForMerchantAsync(merchantData.MerchantId, token);
                        }
                        else
                        {
                            token = unusedToken.Value;
                        }

                        Random generator = new Random();
                        String randomOTP = generator.Next(0, 1000000).ToString("D6");
                        var saveOTP = await _merchantWebRepository.AddGeneratedOTP(randomOTP, token);

                        var html = GetRegistrationHTML
                        (
                           html: System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "templates/RegistrationOtp.html")),
                           userName: $"{model.FirstName}",
                           otp: randomOTP,
                           email: model.Email
                        );

                        var mail = CommonLogic.SendEmail
                        (
                           subject: $"Password reset for - {model.FirstName}",
                           body: html,
                           toEmail: model.Email
                        );

                        OTPModel oTPModel = new OTPModel()
                        {
                            MerchantId = merchantData.MerchantId == null ? 0 : merchantData.MerchantId,
                            EmailAddress = model.Email,
                            token = token
                        };

                        if (string.IsNullOrWhiteSpace(mail))
                        {
                            TempData[MessageConstant.SuccessMessageKey] = MessageConstant.RegisterEmailSuccess;
                            return RedirectToAction("SubmitOTP", oTPModel);
                        }
                        else
                        {
                            TempData[MessageConstant.ErrorMessageKey] = MessageConstant.RegisterEmailFailed;
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        TempData[MessageConstant.ErrorMessageKey] = MessageConstant.RegisterEmailFailed;
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Submits the otp.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <param name="tokenId">The token identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SubmitOTP(OTPModel otplList)
        {
            BindMessages();
            ViewBag.merchantId = otplList.MerchantId;
            ViewBag.EmailAddress = otplList.EmailAddress;
            ViewBag.tokenId = otplList.token;
            return View();
        }

        /// <summary>
        /// Submits the otp.
        /// </summary>
        /// <param name="otplList">The otpl list.</param>
        /// <param name="SubmittedOTP">The submitted otp.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SubmitOTP(OTPModel otplList, long submittedOTP)
        {
            var MerchantURL = _configuration["Urls:MerchantPortalURL"];
            long orginalOTP = await _merchantWebRepository.GetGeneratedOTP(otplList.token);
            if (orginalOTP > 0)
            {
                if (orginalOTP != submittedOTP)
                {
                    return Json(new
                    {
                        success = false,
                        message = MessageConstant.OTPNotMatch,
                        data = 0,
                        url = "",
                    });
                }
                else
                {
                    bool UpdateActiveMerchantClinic = await _merchantWebRepository.UpdateMerchantClinicByIdAsync(Convert.ToInt64(otplList.MerchantId), submittedOTP);
                    if (UpdateActiveMerchantClinic)
                    {
                        return Json(new
                        {
                            success = true,
                            message = MessageConstant.OTPSuccesfullyValidated,
                            data = otplList.MerchantId,
                            url = MerchantURL
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = MessageConstant.ErrorUpdateMerchant,
                            data = 0,
                            url = "",
                        });
                    }
                }
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = MessageConstant.OTPNotFound,
                    data = 0,
                    url = "",
                });
            }
        }

        /// <summary>
        /// Resends the otp.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ResendOTP(Guid token, long merchantId)
        {
            Random generator = new Random(); OTPModel oTPModel = new OTPModel();
            String randomOTP = generator.Next(0, 1000000).ToString("D6");

            var UpdateOldOTP = await _merchantWebRepository.UpdatedOTP(token);
            if (UpdateOldOTP)
            {
                var saveOTP = await _merchantWebRepository.AddGeneratedOTP(randomOTP, token);

                var merchantDetails = await _merchantWebRepository.GetMerchantByIdAsync(merchantId);

                var html = GetRegistrationHTML
                (
                   html: System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "templates/RegistrationOtp.html")),
                   userName: $"{merchantDetails.FirstName}",
                   otp: randomOTP,
                   email: merchantDetails.EmailAddress
                );

                var mail = CommonLogic.SendEmail
                (
                   subject: $"Password reset for - {merchantDetails.FirstName}",
                   body: html,
                   toEmail: merchantDetails.EmailAddress
                );


                oTPModel = new OTPModel()
                {
                    EmailAddress = merchantDetails.EmailAddress,
                    token = token,
                    MerchantId = merchantId
                };
            }
            return RedirectToAction("SubmitOTP", oTPModel);
        }

        /// <summary>
        /// Adds the bank details.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddBankDetails(long merchantId)
        {
            var MerchantURL = _configuration["Urls:MerchantPortalURL"];

            BindMessages();
            ViewBag.merchantId = merchantId;
            ViewBag.merchantURL = MerchantURL;
            return View();
        }

        /// <summary>
        /// Adds the bank details.
        /// </summary>
        /// <param name="merchantBankDetails">The merchant bank details.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddBankDetails(MerchantBankDetails merchantBankDetails)
        {
            try
            {
                ViewBag.merchantId = merchantBankDetails.merchantId;
                if (!ModelState.IsValid)
                {
                    return View(merchantBankDetails);
                }
                else
                {
                    var addMerchantBank = await _merchantWebRepository.AddMerchantBank(merchantBankDetails);
                    if (addMerchantBank)
                    {
                        TempData[MessageConstant.SuccessMessageKey] = MessageConstant.MerchantBankSuccess;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData[MessageConstant.ErrorMessageKey] = MessageConstant.MerchantBankFailure;
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// Gets the bank detailby BSB.
        /// </summary>
        /// <param name="bsbNumber">The BSB number.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetBankDetailbyBSB(string bsbNumber)
        {
            ValidateBSBNumberRequest validateBSBNumberRequest = new ValidateBSBNumberRequest()
            {
                BSBNumber = bsbNumber
            };

            var validateBSB = await _bankRepository.IsValidateBSBNumberForAPI(bsbNumber);
            if(!validateBSB)
            {
                return Json(new
                {
                    success = false,
                    message = MessageConstant.InValidBSB,
                    data = "",
                });
            }
            else
            {
                var (isSuccess, message, ValidateBSBNumberResponse) = await _bankRepository.GetBankDetailAfterBSBValidationForAPI(validateBSBNumberRequest);
                if(isSuccess)
                {
                    return Json(new
                    {
                        success = isSuccess,
                        message = message,
                        data = ValidateBSBNumberResponse,
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = isSuccess,
                        message = message,
                        data = "",
                    });
                }
            }
        }

        /// <summary>
        /// Gets the service dropdown list by category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetServiceDropdownListByCategory(long id)
        {
            try
            {
                var serviceDetail = await _merchantWebRepository.GetActiveServiceDropdownListAsync(id);
                return Json(new
                {
                    success = true,
                    serviceDetail,
                });
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the forget password HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="passwordResetToken">The password reset token.</param>
        /// <returns></returns>
        private string GetForgetPasswordHTML(string html, string userName, string passwordResetToken)
        {
            try
            {
                var domain = $"{Request.Scheme}://{Request.Host}";
                html = html.Replace("##FULLNAME##", userName);
                html = html.Replace("##DOMAIN##", domain);
                return html.Replace("##RESETPASSWORDLINK##", $"{Url.RouteUrl("resetPassword", new { token = passwordResetToken }, Request.Scheme.ToString(), Request.Host.ToString())}");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the registration HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="otp">The otp.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        private string GetRegistrationHTML(string html, string userName, string otp, string email)
        {
            try
            {
                var domain = $"{Request.Scheme}://{Request.Host}";
                html = html.Replace("##FULLNAME##", userName);
                html = html.Replace("##DOMAIN##", domain);
                html = html.Replace("##OTP##", otp);
                return html;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///   Logouts this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            try
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }

        #endregion 
    }
}