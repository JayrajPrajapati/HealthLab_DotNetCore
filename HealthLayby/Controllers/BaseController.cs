using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    ///   base controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class BaseController : Controller
    {
        #region Protected property

        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        protected readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// The claim
        /// </summary>
        protected readonly ClaimModel claim;

        #endregion

        #region Constructor

        /// <summary>
        ///   Initializes a new instance of the <see cref="BaseController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            claim = new ClaimModel(httpContextAccessor);
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Protected functions

        /// <summary>
        ///   Binds the messages.
        /// </summary>
        protected void BindMessages()
        {
            if (TempData[MessageConstant.SuccessMessageKey] is not null)
                ViewData[MessageConstant.SuccessMessageKey] = TempData[MessageConstant.SuccessMessageKey];

            if (TempData[MessageConstant.ErrorMessageKey] is not null)
                ViewData[MessageConstant.ErrorMessageKey] = TempData[MessageConstant.ErrorMessageKey];
        }

        /// <summary>
        ///   Updates the first name and last name in claim.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        protected async Task UpdateFirstNameAndLastNameInClaim(string firstName, string lastName)
        {
            ClaimsPrincipal principal = new ClaimsPrincipal
            (
                identity: new ClaimsIdentity
                (
                    claims: new List<Claim>
                    {
                            new Claim("AdminId", claim.AdminId.ToString()),
                            new Claim("FullName", $"{firstName} {lastName}"),
                            new Claim("FirstName", firstName),
                            new Claim("LastName", lastName),
                            new Claim("EmailAddress", claim.EmailAddress)
                    },
                    authenticationType: CookieAuthenticationDefaults.AuthenticationScheme
                )
            );

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
            {
                ExpiresUtc = DateTime.UtcNow.AddDays(10)

            });
        }

        #endregion
    }
}
