using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Merchant.Controllers
{
    /// <summary>
    /// MerchantLayByPlans
    /// </summary>
    /// <seealso cref="HealthLayby.Merchant.Controllers.BaseController" />
    public class MerchantLayByPlansController : BaseController
    {
        #region Private Variables

        /// <summary>
        /// The merchant plans web repository
        /// </summary>
        private readonly IMerchantPlansWebRepository _merchantPlansWebRepository;

        /// <summary>
        /// The merchant repository
        /// </summary>
        private readonly IMerchantWebRepository _merchantWebRepository;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        ///   The env
        /// </summary>
        private readonly IWebHostEnvironment _env;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantLayByPlansController" /> class.
        /// </summary>
        /// <param name="merchantPlansWebRepository">The merchant plans web repository.</param>
        /// <param name="merchantWebRepository">The merchant web repository.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public MerchantLayByPlansController(IMerchantPlansWebRepository merchantPlansWebRepository, IMerchantWebRepository merchantWebRepository, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _merchantPlansWebRepository = merchantPlansWebRepository;
            _merchantWebRepository = merchantWebRepository;
            _configuration = configuration;
            _env = webHostEnvironment;
        }

        #endregion

        #region Public Method's

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            BindMessages();
            var getPlans = await _merchantPlansWebRepository.GetAllPlansList(claim.AdminId);
            ViewBag.PlanTotalAmount = getPlans.Count > 0 ? getPlans.Sum(x => x.ServiceAmount) : 0;
            ViewBag.CustomerTotalAmtPaid = getPlans.Count > 0 ? getPlans.Sum(y => y.CustomerTotalAmtPaid) : 0;
            return View(getPlans);
        }

        /// <summary>
        /// Actives the index.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveIndex()
        {
            BindMessages();
            var getPlans = await _merchantPlansWebRepository.GetActivePlansList(claim.AdminId);
            ViewBag.PlanTotalAmount = getPlans.Count > 0 ? getPlans.Sum(x => x.ServiceAmount) : 0;
            ViewBag.CustomerTotalAmtPaid = getPlans.Count > 0 ? getPlans.Sum(y => y.CustomerTotalAmtPaid) : 0;
            return View(getPlans);
        }

        /// <summary>
        /// Completeds the index.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CompletedIndex()
        {
            BindMessages();
            var getPlans = await _merchantPlansWebRepository.GetCompletedPlansList(claim.AdminId);
            ViewBag.PlanTotalAmount = getPlans.Count > 0 ? getPlans.Sum(x => x.ServiceAmount) : 0;
            ViewBag.CustomerTotalAmtPaid = getPlans.Count > 0 ? getPlans.Sum(y => y.CustomerTotalAmtPaid) : 0;
            return View(getPlans);
        }

        /// <summary>
        /// Pauses the index.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PauseIndex()
        {
            BindMessages();
            var getPlans = await _merchantPlansWebRepository.GetPausePlansList(claim.AdminId);
            ViewBag.PlanTotalAmount = getPlans.Count > 0 ? getPlans.Sum(x => x.ServiceAmount) : 0;
            ViewBag.CustomerTotalAmtPaid = getPlans.Count > 0 ? getPlans.Sum(y => y.CustomerTotalAmtPaid) : 0;
            return View(getPlans);
        }

        /// <summary>
        /// Cancels the index.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> CancelIndex()
        {
            BindMessages();
            var getPlans = await _merchantPlansWebRepository.GetCancelPlansList(claim.AdminId);
            ViewBag.PlanTotalAmount = getPlans.Count > 0 ? getPlans.Sum(x => x.ServiceAmount) : 0;
            ViewBag.CustomerTotalAmtPaid = getPlans.Count > 0 ? getPlans.Sum(y => y.CustomerTotalAmtPaid) : 0;
            return View(getPlans);
        }

        /// <summary>
        /// Gets the plans details.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="indexName">Name of the index.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPlansDetails(long id)
        {
            try
            {
                return View("_View", await _merchantPlansWebRepository.GetPlansDetailsById(id) ?? new MerchantPlansModel());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sends the otp.
        /// </summary>
        /// <param name="customerEmail">The customer email.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SendOTP(string merchantName,string customerName,long planId)
        {
            try
            {
                var isMerchantExists = await _merchantPlansWebRepository.IsMerchantExists(merchantName);
                if (isMerchantExists != null)
                {
                    var token = Guid.NewGuid();
                    var unusedToken = await _merchantWebRepository.GetUnusedTokenForMerchantAsync(isMerchantExists.MerchantId);

                    if (unusedToken is null || unusedToken == Guid.Empty)
                    {
                        await _merchantWebRepository.AddForgotPasswordTokenForMerchantAsync(isMerchantExists.MerchantId, token);
                    }
                    else
                    {
                        token = unusedToken.Value;
                    }

                    var customer = await _merchantPlansWebRepository.GetCustomerDetails(customerName);

                    Random generator = new Random();
                    String randomOTP = generator.Next(0, 1000000).ToString("D6");
                    var saveOTP = await _merchantWebRepository.AddGeneratedOTP(randomOTP, token);

                    var html = SendOTPHTML
                    (
                       html: System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "templates/CompleteOtp.html")),
                       userName: $"{customerName}",
                       otp: randomOTP,
                       email: customer != null ? customer.EmailAddress : ""
                    );

                    var mail = CommonLogic.SendEmail
                    (
                       subject: $"Complete Plan OTP - {customerName}",
                       body: html,
                       toEmail: customer != null ? customer.EmailAddress : ""
                    );

                    OTPModel oTPModel = new OTPModel()
                    {
                        MerchantId = isMerchantExists.MerchantId,
                        EmailAddress = customer != null ? customer.EmailAddress : "",
                        token = token
                    };

                    return Json(new
                    {
                        success = true,
                        message = MessageConstant.SuccessfullSendOTP,
                        data = planId,
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = MessageConstant.UnSuccessfullSendOTP,
                    });
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Submits the otp.
        /// </summary>
        /// <param name="customerPlanId">The customer plan identifier.</param>
        /// <returns></returns>
        public async Task<IActionResult> SubmitOTP(long Id,long submittedOTP)
        {
            try
            {
                var plan = await _merchantPlansWebRepository.UpdatePlan(Id, submittedOTP, claim.AdminId);
                if (plan)
                {
                    return Json(new
                    {
                        success = true,
                        message = MessageConstant.ValidPlanOTP,
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = MessageConstant.InValidPlanOTP,
                    });
                }
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region private

        /// <summary>
        /// Gets the registration HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="otp">The otp.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        private string SendOTPHTML(string html, string userName, string otp, string email)
        {
            try
            {
                var domain = $"{Request.Scheme}://{Request.Host}";
                html = html.Replace("##FULLNAME##", userName);
                html = html.Replace("##DOMAIN##", domain);
                html = html.Replace("##OTP##", otp);
                return html;
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}