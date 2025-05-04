using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Merchant.Controllers
{
    /// <summary>
    /// MerchantProfileController
    /// </summary>
    /// <seealso cref="HealthLayby.Merchant.Controllers.BaseController" />
    [Authorize]
    public class MerchantProfileController : BaseController
    {
        #region Private Variables

        /// <summary>
        /// The merchant profile web repository
        /// </summary>
        private readonly IMerchantProfileWebRepository _merchantProfileWebRepository;

        /// <summary>
        ///   The env
        /// </summary>
        private readonly IWebHostEnvironment _env;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantProfileController" /> class.
        /// </summary>
        /// <param name="merchantProfileWebRepository">The merchant profile web repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public MerchantProfileController(IMerchantProfileWebRepository merchantProfileWebRepository, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _merchantProfileWebRepository = merchantProfileWebRepository;
            _env = env;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                BindMessages();
                var model= _merchantProfileWebRepository.GetMerchantProfile(claim.AdminId);
                return View(model);
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="merchentProfileModel">The merchent profile model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateProfile(MerchentProfileModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData[MessageConstant.ErrorMessageKey] = MessageConstant.MerchantProfileError;
                }
                else
                {
                    if (model.ImageBase64 is not null && model.ImageFileExtension is not null)
                    {
                        model.profileImage = FileUploadHelper.UploadFile
                        (
                            base64: model.ImageBase64,
                            extension: model.ImageFileExtension,
                            path: Path.Combine(_env.WebRootPath, DirectoryConstant.MerchantProfilePicDirectory)
                        );
                    }

                    if (model.LogoBase64 is not null && model.LogoFileExtension is not null)
                    {
                        model.Logo = FileUploadHelper.UploadFile
                        (
                            base64: model.LogoBase64,
                            extension: model.LogoFileExtension,
                            path: Path.Combine(_env.WebRootPath, DirectoryConstant.MerchantLogoDirectory)
                        );
                    }

                    var updateProfile = await _merchantProfileWebRepository.UpdateMerchantProfile(model, claim.AdminId);
                    if (updateProfile)
                    {
                        await UpdateFirstNameAndLastNameInClaim(model.firstName, model.lastName);
                        TempData[MessageConstant.SuccessMessageKey] = MessageConstant.MerchantProfileSuccess;
                    }

                    else
                        TempData[MessageConstant.ErrorMessageKey] = MessageConstant.MerchantProfileError;
                }
                return RedirectToAction("Index", "MerchantDashboard");
            }
            catch
            {
                throw;
            }            
        }

        #endregion
    }
}
