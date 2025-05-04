using HealthLayby.Helpers.Constant;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace HealthLayby.Merchant.Controllers
{
    /// <summary>
    /// MerchantTermsConditionController
    /// </summary>
    /// <seealso cref="HealthLayby.Merchant.Controllers.BaseController" />
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Authorize]
    public class MerchantTermsConditionController : BaseController
    {
        #region Private Variables

        /// <summary>
        /// The merchant terms web repository
        /// </summary>
        private readonly IMerchantTermsWebRepository _merchantTermsWebRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantTermsConditionController"/> class.
        /// </summary>
        /// <param name="merchantTermsWebRepository">The merchant terms web repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public MerchantTermsConditionController(IMerchantTermsWebRepository merchantTermsWebRepository, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _merchantTermsWebRepository = merchantTermsWebRepository;
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
            var model = await _merchantTermsWebRepository.GetMerchantTerms();
            return View(model);
        }

        /// <summary>
        /// Edits the terms condition.
        /// </summary>
        /// <param name="termsConditionModel">The terms condition model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditTermsCondition(TermsConditionModel termsConditionModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Json(new { success = false, message = MessageConstant.InvalidModalState });
                }
                var (isSuccess, message) = await _merchantTermsWebRepository.SaveTermsConditionAsync(termsConditionModel, claim.AdminId);

                return Json(new { success = isSuccess, message });
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
