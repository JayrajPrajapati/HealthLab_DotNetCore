using HealthLayby.Models.ApiViewModels.Bank.Response;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Merchant.Controllers
{
    /// <summary>
    /// MerchantServicesController
    /// </summary>
    /// <seealso cref="HealthLayby.Merchant.Controllers.BaseController" />
    public class MerchantServicesController : BaseController
    {
        #region Private Variables

        /// <summary>
        /// The merchant services web repository
        /// </summary>
        private readonly IMerchantServicesWebRepository _merchantServicesWebRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantServicesController"/> class.
        /// </summary>
        /// <param name="merchantServicesWebRepository">The merchant services web repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public MerchantServicesController(IMerchantServicesWebRepository merchantServicesWebRepository, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _merchantServicesWebRepository = merchantServicesWebRepository;
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
            List<MerchantServiceModel> model = await _merchantServicesWebRepository.GetServiceDetailsByLogin(claim.AdminId);
            return View(model);
        }

        /// <summary>
        /// Gets the service details by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetServiceDetailsById(long Id)
        {
            try
            {
                var model = await _merchantServicesWebRepository.GetServiceDetailsBySerivceId(Id);
                if (model.ServiceId > 0)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Successfully fetch details.",
                        data = model,
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Falied in fetching details.",
                        data = "",
                    });
                }
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// Updates the service.
        /// </summary>
        /// <param name="merchantServiceModel">The merchant service model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateService(MerchantServiceModel merchantServiceModel)
        {
            try
            {
                var update = await _merchantServicesWebRepository.UpdateService(merchantServiceModel, claim.AdminId);
                if (update)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Successfully updated service!",
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Falied in updating services!",
                    });
                }
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// Updates the service details by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UpdateServiceDetailsById(long Id)
        {
            try
            {
                var model = await _merchantServicesWebRepository.GetServiceDetailsBySerivceId(Id);
                if (model.ServiceId > 0)
                {
                    var updateService = await _merchantServicesWebRepository.UpdateServiceStatus(Id, claim.AdminId);
                    if (updateService)
                    {
                        return Json(new
                        {
                            success = true,
                            message = "Successfully updated status !",
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Error in updating status!",
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = "Error in updating status!",
                    });
                }
            }
            catch
            {

                throw;
            }
        }

        #endregion
    }
}
