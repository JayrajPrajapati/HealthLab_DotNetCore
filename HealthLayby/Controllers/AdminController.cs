using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    ///   admincontroller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    public class AdminController : BaseController
    {

        #region Private Varibale 

        /// <summary>
        ///   The admin repository
        /// </summary>
        private readonly IAdminRepository _adminRepository;

        #endregion

        #region Constructor

        /// <summary>
        ///   Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="adminRepository">The admin repository.</param>
        public AdminController(IHttpContextAccessor httpContextAccessor, IAdminRepository adminRepository) : base(httpContextAccessor)
        {
            _adminRepository = adminRepository;
        }

        #endregion

        #region Public Method

        /// <summary>
        ///   Updates the profile.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UpdateProfile()
        {
            try
            {
                BindMessages();
                UpdateAdminProfileModel adminProfile = new UpdateAdminProfileModel();
                var user = await _adminRepository.GetAdminByAdminIdAsync(claim.AdminId);

                if (user != null)
                {
                    adminProfile.AdminId = user.AdminId;
                    adminProfile.FirstName = user.FirstName;
                    adminProfile.LastName = user.LastName;
                    adminProfile.EmailAddress = user.EmailAddress;
                }
                return View(adminProfile);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///   Updates the profile.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdateProfile(UpdateAdminProfileModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.InvalidModalState;
                    return RedirectToAction();
                }

                var (isSuccess, message) = await _adminRepository.UpdateAdminProfileAsync(claim.AdminId, model.FirstName, model.LastName);

                if (!isSuccess)
                {
                    TempData[MessageConstant.ErrorMessageKey] = message;
                    return RedirectToAction();
                }

                await UpdateFirstNameAndLastNameInClaim(model.FirstName, model.LastName);

                TempData[MessageConstant.SuccessMessageKey] = message;
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///   Changes the password.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        ///   Changes the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.InvalidModalState;
                    return RedirectToRoute("changePassword");
                }

                var (isSuccess, message) = await _adminRepository.UpdateAdminPasswordAsync(claim.AdminId, model);

                if (!isSuccess)
                {
                    TempData[MessageConstant.ErrorMessageKey] = message;
                    return RedirectToRoute("changePassword");
                }

                TempData[MessageConstant.SuccessMessageKey] = message;
                return RedirectToAction("Logout", "Login");
            }
            catch
            {
                throw;
            }
        }

        //[HttpPost]
        //public Task<IActionResult> IsAuhenticated()
        //{
        //    return JsonResult(true);
        //}

        #endregion
    }
}
