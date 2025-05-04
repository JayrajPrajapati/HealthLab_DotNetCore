using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.ApiViewModels.Bank.Request;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// merchantController
    /// </summary>
    /// <seealso cref="HealthLayby.Admin.Controllers.BaseController" />
    [Authorize]
    public class MerchantController : BaseController
    {
        #region Private Variable 

        /// <summary>
        /// The merchant repository
        /// </summary>
        private readonly IMerchantRepository _merchantRepository;

        /// <summary>
        /// The env
        /// </summary>
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The general repository
        /// </summary>
        private readonly IGeneralRepository _generalRepository;

        /// <summary>
        /// The bank repository
        /// </summary>
        private readonly IBankRepository _bankRepository;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantController" /> class.
        /// </summary>
        /// <param name="merchantRepository">The merchant repository.</param>
        /// <param name="env">The env.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="generalRepository">The general repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public MerchantController(IMerchantRepository merchantRepository,
                                    IWebHostEnvironment env,
                                    IConfiguration configuration,
                                    IGeneralRepository generalRepository,
                                    IHttpContextAccessor httpContextAccessor, IBankRepository bankRepository) : base(httpContextAccessor)
        {
            _merchantRepository = merchantRepository;
            _env = env;
            _configuration = configuration;
            _generalRepository = generalRepository;
            _bankRepository = bankRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the merchant list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetMerchantList()
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
                    "1" => "Merchant",
                    "2" => "Email",
                    "3" => "Phone",
                    "4" => "Clinic/Lab",
                    "5" => "Location",
                    "6" => "Status",
                    _ => "CreatedDate",
                };

                var (data, totalRecord, totalFilteredRecord) = await _merchantRepository.GetMerchantListAsync
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
                    merchantCount = totalRecord,
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
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AddEdit(long id = 0)
        {
            try
            {
                return View("_AddEdit", await _merchantRepository.GetMerchantByIdAsync(id) ?? new MerchantModel() { IsActive = true });
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
        public async Task<IActionResult> Save(MerchantModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = MessageConstant.InvalidModalState });
                }

                #region File Upload

                if (model.MerchantId > 0)
                {
                    var merchant = await _merchantRepository.GetMerchantByIdAsync(model.MerchantId);

                    if (merchant is not null)
                    {
                        var allowDeleteProfilePic = !string.IsNullOrWhiteSpace(model.ImageBase64)
                                                 && !string.IsNullOrWhiteSpace(model.ImageFileExtension);

                        if (allowDeleteProfilePic && !string.IsNullOrWhiteSpace(merchant.ProfilePic))
                        {
                            FileUploadHelper.DeleteFile
                            (
                                path: Path.Combine(_env.WebRootPath, DirectoryConstant.MerchantProfilePicDirectory, merchant.ProfilePic)
                            );
                        }

                        var allowDeleteLogoPic = !string.IsNullOrWhiteSpace(model.LogoBase64)
                                                 && !string.IsNullOrWhiteSpace(model.LogoFileExtension);

                        if (allowDeleteLogoPic && !string.IsNullOrWhiteSpace(merchant.Logo))
                        {
                            FileUploadHelper.DeleteFile
                            (
                                path: Path.Combine(_env.WebRootPath, DirectoryConstant.MerchantLogoDirectory, merchant.Logo)
                            );
                        }
                    }
                }

                if (model.ImageBase64 is not null && model.ImageFileExtension is not null)
                {
                    model.ProfilePic = FileUploadHelper.UploadFile
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

                #endregion

                var password = model.MerchantId > 0 ? null : Password.Generate(10, 4);

                var (isSuccess, message) = await _merchantRepository.SaveMerchantAsync(model, claim.AdminId, password);

                if (isSuccess && model.MerchantId <= 0 && !string.IsNullOrWhiteSpace(password))
                {
                    SendEmailToCreatedMerchant
                    (
                        userName: $"{model.FirstName} {model.LastName}",
                        email: model.EmailAddress,
                        password: password
                    );
                }
                return Json(new { success = isSuccess, message });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Views the merchant.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ViewMerchant(long id = 0)
        {
            try
            {
                return View("_View", await _merchantRepository.GetMerchantByIdAsync(id) ?? new MerchantModel());
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
        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var (isSuccess, message) = await _merchantRepository.DeleteMerchantAsync(id, claim.AdminId);

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
        /// Determines whether [is merchant email exist] [the specified merchantid].
        /// </summary>
        /// <param name="merchantid">The merchantid.</param>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> IsMerchantEmailExist(long? merchantid, string emailAddress)
        {
            try
            {
                return Json(!await _merchantRepository.IsMerchantEmailExistAsync(merchantid, emailAddress));
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Gets the service dropdown list.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetServiceDropdownList(long categoryId)
        {
            try
            {
                return Json(await _generalRepository.GetActiveServiceDropdownListAsync(categoryId));
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Views the pay merchant.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ViewPayMerchant(long id = 0)
        {
            try
            {
                return View("_PayMerchant", await _merchantRepository.GetMerchantByIdAsync(id) ?? new MerchantModel());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the bank detailby BSB.
        /// </summary>
        /// <param name="bsbNumber">The BSB number.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetBankDetailbyBSB(string bsbNumber)
        {
            ValidateBSBNumberRequest validateBSBNumberRequest = new ValidateBSBNumberRequest()
            {
                BSBNumber = bsbNumber
            };

            var validateBSB = await _bankRepository.IsValidateBSBNumberForAPI(bsbNumber);
            if (!validateBSB)
            {
                return Json(new
                {
                    success = false,
                    message = MessageConstant.InValidBSB,
                    data = "",
                });
            }
            else
            {
                var (isSuccess, message, ValidateBSBNumberResponse) = await _bankRepository.GetBankDetailAfterBSBValidationForAPI(validateBSBNumberRequest);
                if (isSuccess)
                {
                    return Json(new
                    {
                        success = isSuccess,
                        message = message,
                        data = ValidateBSBNumberResponse,
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = isSuccess,
                        message = message,
                        data = "",
                    });
                }
            }
        }
        #endregion

        #region Private Method

        /// <summary>
        /// Gets the create merchant HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        private string GetCreateMerchantHTML(string html, string userName, string password, string email)
        {
            try
            {
                var domain = $"{Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host.ToString()}";
                var acceptMerchantURL = _configuration["Urls:MerchantPortalURL"];

                html = html.Replace("##FULLNAME##", userName);
                html = html.Replace("##USERNAME##", email);
                html = html.Replace("##PASSWORD##", password);
                html = html.Replace("##URL##", acceptMerchantURL);
                html = html.Replace("##DOMAIN##", domain);

                return html;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sends the email to created merchant.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        private void SendEmailToCreatedMerchant(string userName, string email, string password)
        {
            var html = GetCreateMerchantHTML
            (
               html: System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "templates/CreateMerchant.html")),
               userName: $"{userName}",
               email: email,
               password: password
            );

            CommonLogic.SendEmail
            (
               subject: $"Your Health Layby Application has been created.",
               body: html,
               toEmail: email
            );

        }

        #endregion
    }
}
