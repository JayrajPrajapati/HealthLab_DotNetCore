using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// login controller
    /// </summary>
    /// <seealso cref="HealthLayby.Admin.Controllers.BaseController" />
    public class LoginController : BaseController
    {
        #region Private Variable

        /// <summary>
        /// The admin repository
        /// </summary>
        private readonly IAdminRepository _adminRepository;

        /// <summary>
        /// The forgot password repository
        /// </summary>
        private readonly IForgotPasswordRepository _forgotPasswordRepository;

        /// <summary>
        /// The env
        /// </summary>
        private readonly IWebHostEnvironment _env;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="adminRepository">The admin repository.</param>
        /// <param name="forgotPasswordRepository">The forgot password repository.</param>
        /// <param name="env">The env.</param>
        public LoginController(IHttpContextAccessor httpContextAccessor,
                               IAdminRepository adminRepository,
                               IForgotPasswordRepository forgotPasswordRepository,
                               IWebHostEnvironment env
                              ) : base(httpContextAccessor)
        {
            _adminRepository = adminRepository;
            _forgotPasswordRepository = forgotPasswordRepository;
            _env = env;
        }

        #endregion

        #region Public Methods

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
                    return RedirectToAction("Index", "Dashboard");
                }
                return View();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Indexes the specified admin model.
        /// </summary>
        /// <param name="adminModel">The admin model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginModel adminModel, string? returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(adminModel);
                }

                var data = await _adminRepository.GetAdminByEmailAddressAsync(adminModel.EmailAddress);

                if (data is null)
                {
                    ModelState.AddModelError(nameof(adminModel.Password), MessageConstant.InValidEmailAndPassword);
                    return View(adminModel);
                }

                if (CommonLogic.Encrypt(adminModel.Password) != data.Password)
                {
                    ModelState.AddModelError(nameof(adminModel.Password), MessageConstant.InValidPassword);
                    return View("Index", adminModel);
                }

                var principal = new ClaimsPrincipal
                (
                    identity: new ClaimsIdentity
                    (
                        claims: new List<Claim>
                        {
                              new Claim("AdminId", data.AdminId.ToString()),
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

                return RedirectToAction("Index", "Dashboard");
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Forgots the password.
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
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.InvalidModalState;
                    return RedirectToRoute("forgotPassword");
                }

                var adminData = await _adminRepository.GetAdminByEmailAddressAsync(model.EmailAddress);
                if (adminData is null)
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.AdminNotFound;
                    return RedirectToRoute("forgotPassword");
                }

                var token = Guid.NewGuid();
                var unusedToken = await _forgotPasswordRepository.GetUnusedTokenForAdminAsync(adminData.AdminId);

                if (unusedToken is null || unusedToken == Guid.Empty)
                {
                    await _forgotPasswordRepository.AddForgotPasswordTokenForAdminAsync(adminData.AdminId, token);
                }
                else
                {
                    token = unusedToken.Value;
                }

                var html = GetForgetPasswordHTML
                (
                   html: System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "templates/ForgotPassword.html")),
                   userName: $"{adminData.FullName}",
                   passwordResetToken: token.ToString()
                );

                var mail = CommonLogic.SendEmail
                (
                   subject: $"Password reset for - {adminData.FullName}",
                   body: html,
                   toEmail: adminData.EmailAddress
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
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.InValidResetToken;
                    return RedirectToAction(nameof(Index));
                }

                if (!Guid.TryParse(token, out var _))
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.ExpiredResetToken;
                    return RedirectToAction(nameof(Index));
                }

                bool isTokenValid = await _forgotPasswordRepository.CheckAdminTokenAsync(new Guid(token));

                if (!isTokenValid)
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.ExpiredResetToken;
                    return RedirectToAction(nameof(Index));
                }

                BindMessages();
                return View(new ResetPasswordModel { ResetToken = token });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.InvalidModalState;
                    return RedirectToRoute("forgotPassword");
                }

                var AdminId = await _forgotPasswordRepository.GetAdminIdByTokenAsync(new Guid(model.ResetToken));
                if (AdminId.HasValue)
                {
                    var adminData = await _adminRepository.GetAdminByAdminIdAsync(AdminId.Value);
                    if (adminData is null)
                    {
                        TempData[MessageConstant.ErrorMessageKey] = MessageConstant.ExpiredResetToken;
                        return RedirectToAction(nameof(ResetPassword), new { token = model.ResetToken });
                    }

                    await _adminRepository.UpdateAdminResetPasswordAsync(adminData.AdminId, CommonLogic.Encrypt(model.Password));

                    await _forgotPasswordRepository.UpdateChangeDateAsync(new Guid(model.ResetToken));

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
