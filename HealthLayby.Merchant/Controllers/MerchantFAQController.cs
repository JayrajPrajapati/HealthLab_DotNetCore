using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Merchant.Controllers
{
    /// <summary>
    /// MerchantFAQController
    /// </summary>
    /// <seealso cref="HealthLayby.Merchant.Controllers.BaseController" />
    [Authorize]
    public class MerchantFAQController : BaseController
    {
        #region Private Variables

        /// <summary>
        /// The merchant FAQ web repository
        /// </summary>
        private readonly IMerchantFaqWebRepository _merchantFaqWebRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantDashboardController" /> class.
        /// </summary>
        /// <param name="merchantFaqWebRepository">The merchant FAQ web repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public MerchantFAQController(IMerchantFaqWebRepository merchantFaqWebRepository, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _merchantFaqWebRepository = merchantFaqWebRepository;
        }

        #endregion

        #region  Public Methods

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            BindMessages();

            var model = await _merchantFaqWebRepository.GetMerchantFaq();
            return View(model);
        }


        /// <summary>
        ///   Adds the edit FAQ.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddEditMerchantFAQ(long id = 0)
        {
            try
            {
                return PartialView("_AddEditFAQ", await _merchantFaqWebRepository.GetFAQByIdAsync(id) ?? new FAQMerchantModel());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///   Saves the FAQ.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveMerchantFAQ(FAQMerchantModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = MessageConstant.InvalidModalState });
                }

                ViewBag.IsFromView = true;

                var (isSuccess, message) = await _merchantFaqWebRepository.SaveFAQAsync(model, claim.AdminId);

                return Json(new { success = isSuccess, message });
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        ///   Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(long id, string rejectreason)
        {
            try
            {
                var (isSuccess, message) = await _merchantFaqWebRepository.DeleteFAQAsync(id, claim.AdminId, rejectreason);

                return Json(new
                {
                    success = isSuccess,
                    message
                });
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
