using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// content management controller
    /// </summary>
    /// <seealso cref="HealthLayby.Admin.Controllers.BaseController" />
    [Authorize]
    public class ContentManagementController : BaseController
    {

        /// <summary>
        /// The CMS repository
        /// </summary>
        private readonly ICMSRepository _cmsRepository;


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManagementController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="cmsRepository">The CMS repository.</param>
        public ContentManagementController(IHttpContextAccessor httpContextAccessor,
                                            ICMSRepository cmsRepository) : base(httpContextAccessor)
        {
            _cmsRepository = cmsRepository;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Edits the content.
        /// </summary>
        /// <param name="contentManagement">The content management.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditContent(CMSModel contentManagement)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = MessageConstant.InvalidModalState });
                }

                var (isSuccess, message) = await _cmsRepository.SaveContentAsync(contentManagement, claim.AdminId);

                return Json(new { success = isSuccess, message });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adds the edit FAQ.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddEditFAQ(long id = 0)
        {
            try
            {
                return PartialView("_AddEditFAQ", await _cmsRepository.GetFAQByIdAsync(id) ?? new FAQModel());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Saves the FAQ.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveFAQ(FAQModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = MessageConstant.InvalidModalState });
                }

                ViewBag.IsFromView = true;

                var (isSuccess, message) = await _cmsRepository.SaveFAQAsync(model, claim.AdminId);

                return Json(new { success = isSuccess, message });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="rejectreason">The rejectreason.</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(long id, string rejectreason)
        {
            try
            {
                var (isSuccess, message) = await _cmsRepository.DeleteFAQAsync(id, claim.AdminId, rejectreason);

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

        /// <summary>
        /// FAQs the list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> FAQList()
        {
            try
            {
                return PartialView(await _cmsRepository.GetFAQListAsync());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Edits the content.
        /// </summary>
        /// <param name="contentManagementEnum">The content management enum.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddEditContent(ContentManagementEnum contentManagementEnum)
        {
            try
            {
                CMSModel contentManagement = new CMSModel();
                contentManagement = await _cmsRepository.GetPageContentByPageCodeAsync((int)contentManagementEnum);
                if(contentManagement is null)
                {
                    return RedirectToAction("Index");
                }
                return PartialView("_AddEditContent", contentManagement);
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
