using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Merchant.Controllers
{
    /// <summary>
    /// MerchantChangePasswordController
    /// </summary>
    /// <seealso cref="HealthLayby.Merchant.Controllers.BaseController" />
    [Authorize]
    public class MerchantChangePasswordController : BaseController
    {
        #region Private Variables

        private readonly IMerchantPasswordWebRepository _merchantPasswordWebRepository;

        #endregion

        #region Constructor

        public MerchantChangePasswordController(IMerchantPasswordWebRepository merchantPasswordWebRepository, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _merchantPasswordWebRepository = merchantPasswordWebRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            BindMessages();
            var model = await _merchantPasswordWebRepository.GetMerchantOldPassword(claim.AdminId);
            model.Password = CommonLogic.Decrypt(model.Password);
            return View(model);
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="merchantChangePassword">The merchant change password.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ChangePassword(MerchantChangePassword merchantChangePassword)
        {
            try
            {
                BindMessages();
                if (merchantChangePassword.Password == merchantChangePassword.NewPassword)
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.MerchantOldNewPasswordError;
                }
                else if (merchantChangePassword.NewPassword != merchantChangePassword.ConfirmPassword)
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.MerchantNewConfirmPasswordError;
                }
                else
                {
                    merchantChangePassword.Password = CommonLogic.Encrypt(merchantChangePassword.Password);
                    merchantChangePassword.NewPassword = CommonLogic.Encrypt(merchantChangePassword.NewPassword);
                    var changePassword = await _merchantPasswordWebRepository.ChangePassword(claim.AdminId, merchantChangePassword);
                    if (changePassword)
                        TempData[MessageConstant.SuccessMessageKey] = MessageConstant.MerchantChangePasswordSuccess;
                    else
                        TempData[MessageConstant.ErrorMessageKey] = MessageConstant.MerchantChangePasswordError;
                }
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
