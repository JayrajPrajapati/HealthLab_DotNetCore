using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// service controller
    /// </summary>
    /// <seealso cref="HealthLayby.Admin.Controllers.BaseController" />
    [Authorize]
    public class ServiceController : BaseController
    {
        #region Private Varibale 

        /// <summary>
        /// The merchant repository
        /// </summary>
        private readonly IMerchantRepository _merchantRepository;

        /// <summary>
        /// The service repository
        /// </summary>
        private readonly IServiceRepository _serviceRepository;

        /// <summary>
        /// The env
        /// </summary>
        private readonly IWebHostEnvironment _env;

        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="merchantRepository">The merchant repository.</param>
        /// <param name="serviceRepository">The service repository.</param>
        /// <param name="env">The env.</param>
        public ServiceController(IHttpContextAccessor httpContextAccessor,
                                    IMerchantRepository merchantRepository,
                                    IServiceRepository serviceRepository,
                                    IWebHostEnvironment env) : base(httpContextAccessor)
        {
            _merchantRepository = merchantRepository;
            _serviceRepository = serviceRepository;
            _env = env;
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
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Create()
        {
            return PartialView("_Create", new ServiceModel());
        }

        /// <summary>
        /// Edits the specified identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddEdit(long id = 0)
        {
            try
            {
                return View("_AddEdit", await _serviceRepository.GetServiceModelByIdAsync(id) ?? new ServiceModel());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ServiceModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = MessageConstant.InvalidModalState });
                }

                #region File Upload 

                if (model.ServiceId > 0)
                {
                    var service = await _serviceRepository.GetServiceModelByIdAsync(model.ServiceId);

                    if (service is not null)
                    {
                        var allowDeleteImage = !string.IsNullOrWhiteSpace(model.ImageBase64)
                                                 && !string.IsNullOrWhiteSpace(model.ImageFileExtension);

                        if (allowDeleteImage && !string.IsNullOrWhiteSpace(service.Image))
                        {
                            FileUploadHelper.DeleteFile
                            (
                                path: Path.Combine(_env.WebRootPath, DirectoryConstant.ServiceThumbnailDirectory, service.Image)
                            );
                        }
                    }
                }

                if (model.ImageBase64 is not null && model.ImageFileExtension is not null)
                {
                    model.Image = FileUploadHelper.UploadFile
                    (
                        base64: model.ImageBase64,
                        extension: model.ImageFileExtension,
                        path: Path.Combine(_env.WebRootPath, DirectoryConstant.ServiceThumbnailDirectory)
                    );
                }

                #endregion

                var (isSuccess, message) = await _serviceRepository.SaveServiceAsync(model, claim.AdminId);

                return Json(new { success = isSuccess, message });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the service list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetServiceList()
        {
            try
            {
                Request.Form.TryGetValue("draw", out StringValues draw);
                Request.Form.TryGetValue("order[0][column]", out StringValues orderColumn);
                Request.Form.TryGetValue("order[0][dir]", out StringValues orderDirection);
                Request.Form.TryGetValue("start", out StringValues skipRecord);
                Request.Form.TryGetValue("length", out StringValues pageSize);
                Request.Form.TryGetValue("search[value]", out StringValues searchText);

                string sortingColumnName = orderColumn.ToString() switch
                {
                    "0" => "Service",
                    "1" => "Category",
                    "2" => "Price",
                    "3" => "Merchant",
                    "4" => "Status",
                    _ => "CreatedOn",
                };

                var (data, totalRecord, totalFilteredRecord) = await _serviceRepository.ServiceGridListAsync
                (
                    sortColumn: sortingColumnName,
                    sortOrder: string.IsNullOrWhiteSpace(orderDirection.ToString()) ? "desc" : orderDirection.ToString(),
                    pageSize: Convert.ToInt32(pageSize),
                    pageIndex: Convert.ToInt32(skipRecord),
                    searchText: searchText
                );

                return Json(new
                {
                    draw = Convert.ToInt32(draw),
                    serviceCount = totalRecord,
                    recordsTotal = totalFilteredRecord,
                    recordsFiltered = totalFilteredRecord,
                    data
                });
            }
            catch 
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the service by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<IActionResult> GetServiceById(long id = 0)
        {
            try
            {
                return PartialView("_View", await _serviceRepository.GetServiceModelByIdAsync(id) ?? new ServiceModel());
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
        /// <returns></returns>
        public async Task<IActionResult> Delete(long id = 0)
        {
            try
            {
                var (isSuccess, message) = await _serviceRepository.DeleteServiceAysnc(id, claim.AdminId);
                return Json(new
                {
                    isSuccess,
                    message
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the merchant details by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMerchantDetailsById(long id)
        {
            try
            {
                var merchantDetail = await _merchantRepository.GetMerchantByIdAsync(id) ?? new MerchantModel();
                return Json(new
                {
                    success = true,
                    merchantDetail,
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is service name available] [the specified service name].
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> IsServiceNameAvailable(string serviceName, long serviceId, long categoryId)
        {
            try
            {
                return Json(!await _serviceRepository.IsServiceNameAvailableAsync(serviceName, serviceId, categoryId));
            }
            catch
            {
                throw;
            }
        }

        #endregion

    }
}
