using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// merchantRequestController
    /// </summary>
    /// <seealso cref="HealthLayby.Admin.Controllers.BaseController" />
    [Authorize]
    public class MerchantRequestController : BaseController
    {
        #region Private Variable 

        /// <summary>
        /// The merchant request repository
        /// </summary>
        private readonly IMerchantRequestRepository _merchantRequestRepository;

        /// <summary>
        /// The env
        /// </summary>
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantRequestController" /> class.
        /// </summary>
        /// <param name="merchantRequestRepository">The merchant request repository.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="env">The env.</param>
        /// <param name="configuration">The configuration.</param>
        public MerchantRequestController(IMerchantRequestRepository merchantRequestRepository,
                                                     IHttpContextAccessor httpContextAccessor,
                                                     IWebHostEnvironment env,
                                                     IConfiguration configuration) : base(httpContextAccessor)
        {
            _merchantRequestRepository = merchantRequestRepository;
            _env = env;
            _configuration = configuration;
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
        /// Gets the merchant request list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetMerchantRequestList()
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
                    "2" => "Phone",
                    "3" => "ClinicName",
                    "4" => "Location",
                    "5" => "CategoryName",
                    _ => "CreatedOn",
                };

                var (data, totalRecord, totalFilteredRecord) = await _merchantRequestRepository.GetMerchantRequestListAsync
                (
                    sortColumn: sortingColumnName,
                    sortOrder: string.IsNullOrWhiteSpace(orderDirection) ? "desc" : orderDirection.ToString(),
                    pageSize: Convert.ToInt32(pageSize),
                    pageIndex: Convert.ToInt32(skipRecord),
                    searchText: searchText
                );

                return Json(new
                {
                    draw = Convert.ToInt32(draw),
                    merchantRequestCount = totalRecord,
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
        /// Views the merchant request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ViewMerchantRequest(long id = 0)
        {
            try
            {
                return PartialView("_View", await _merchantRequestRepository.GetMerchantRequestByIdAsync(id) ?? new MerchantModel());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Rejects the merchant request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RejectMerchantRequest(long id = 0)
        {
            try
            {
                return PartialView("_Reject", await _merchantRequestRepository.GetMerchantRequestByIdForRejectRequestAsync(id) ?? new MerchantRequestModel());
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Rejects the request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="rejectedReason">The rejected reason.</param>
        /// <param name="merchantRequestenum">The merchant requestenum.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="serviceIds">The service ids.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> MerchantAcceptRejectRequest([Required] long id, [Required] string rejectedReason, [Required] MerchantRequestEnum merchantRequestenum,long categoryId=0, string serviceIds ="")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = MessageConstant.InvalidModalState });
                }

                var (isSuccess, message) = await _merchantRequestRepository.AcceptAndRejectMerchantRequestAsync(id, claim.AdminId, merchantRequestenum, rejectedReason, categoryId, serviceIds);

                if (isSuccess)
                {
                    var merchant = await _merchantRequestRepository.GetMerchantFullNameAndEmailByIdAsync(id);
                    if (merchant is not null)
                    {
                        if (merchantRequestenum == MerchantRequestEnum.Rejected)
                        {
                            SendEmailToRejetMerchantRequest
                            (
                                userName: $"{merchant.FullName}",
                                email: merchant.EmailAddress,
                                rejectedReason: $"{rejectedReason}"
                            );
                        }
                        else if (merchantRequestenum == MerchantRequestEnum.Accepted)
                        {
                            SendEmailToAcceptMerchantRequest
                            (
                                userName: $"{merchant.FullName}",
                                email: merchant.EmailAddress,
                                password: Password.Generate(10, 4)
                            );
                        }
                    }
                }
                return Json(new { success = isSuccess, message });
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Accepts the merchant request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AcceptMerchantRequest(long id = 0)
        {
            try
            {
                return PartialView("_Accept", await _merchantRequestRepository.GetMerchantRequestByIdForRejectRequestAsync(id) ?? new MerchantRequestModel());
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Private Method

        /// <summary>
        /// Gets the accept merchant HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        private string GetAcceptMerchantHTML(string html, string userName, string password)
        {
            try
            {
                var domain = $"{Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host.ToString()}";
                var acceptMerchantURL = _configuration["Urls:MerchantPortalURL"];

                html = html.Replace("##FULLNAME##", userName);
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
        /// Gets the reject merchant HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="rejectedReason">The rejected reason.</param>
        /// <returns></returns>
        private string GetRejectMerchantHTML(string html, string userName, string rejectedReason)
        {
            try
            {
                var domain = $"{Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host.ToString()}";
                html = html.Replace("##FULLNAME##", userName);
                html = html.Replace("##REJECTEDREASON##", rejectedReason);
                html = html.Replace("##DOMAIN##", domain);

                return html;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sends the email to accept merchant request.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        private void SendEmailToAcceptMerchantRequest(string userName, string email, string password)
        {
            var html = GetAcceptMerchantHTML
            (
               html: System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "templates/AcceptMerchantRequest.html")),
               userName: $"{userName}",
               password: password
            );
            CommonLogic.SendEmail
            (
                subject: $"Your Healthby Application has been Approved.",
                body: html,
                toEmail: email
            );
        }

        /// <summary>
        /// Sends the email to rejet merchant request.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="email">The email.</param>
        /// <param name="rejectedReason">The rejected reason.</param>
        private void SendEmailToRejetMerchantRequest(string userName, string email, string rejectedReason)
        {
            var html = GetRejectMerchantHTML
            (
               html: System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "templates/RejectMerchantRequest.html")),
               userName: $"{userName}",
               rejectedReason: rejectedReason

            );
            CommonLogic.SendEmail
            (
                subject: $"Your Healthby Merchant Application has been Rejected.",
                body: html,
                toEmail: email
            );
        }

        #endregion       

    }
}
