using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels;
using HealthLayby.Repositories.Repositories;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.API.Controllers
{
    /// <summary>
    /// ContentManagementController
    /// </summary>
    /// <seealso cref="HealthLayby.API.Controllers.BaseController" />
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ContentManagementController : BaseController
    {
        #region Private Members

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// The CMS repository
        /// </summary>
        private readonly ICMSRepository _cmsRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentManagementController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="cmsRepository">The CMS repository.</param>
        public ContentManagementController(IHttpContextAccessor httpContextAccessor,
                                            ICMSRepository cmsRepository) : base(httpContextAccessor)
        {
            _cmsRepository = cmsRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <param name="contentManagementEnum">The content management enum.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetContent")]
        public async Task<IActionResult> GetContent(ContentManagementEnum contentManagementEnum)
        {
            try
            {
                var data = await _cmsRepository.GetContentByPageCodeAsyncForAPI(contentManagementEnum);

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status200OK,
                    Message: ApiMessages.DataRetrievedSuccessfully,
                    Data: data
                ));
            }
            catch (Exception ex)
            {
                ErrorLogHelper.APILogError
                (
                    logger: _logger,
                    hostUrl: $"{Request.Scheme}://{Request.Host.Value}{Request.Path}",
                    exception: ex,
                    jwtToken: Request.Headers.TryGetValue("Authorization", out var token) ? token.ToString() : null,
                    requestData: null
                );

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }

        /// <summary>
        /// Gets the FAQ list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFAQList")]
        public async Task<IActionResult> GetFAQList()
        {
            try
            {
                var data = await _cmsRepository.GetFAQListAsyncForAPI();

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status200OK,
                    Message: ApiMessages.DataRetrievedSuccessfully,
                    Data: data
                ));
            }
            catch (Exception ex)
            {
                ErrorLogHelper.APILogError
                (
                    logger: _logger,
                    hostUrl: $"{Request.Scheme}://{Request.Host.Value}{Request.Path}",
                    exception: ex,
                    jwtToken: Request.Headers.TryGetValue("Authorization", out var token) ? token.ToString() : null,
                    requestData: null
                );

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }

        #endregion
    }
}