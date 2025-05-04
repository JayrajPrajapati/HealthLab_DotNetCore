using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels;
using HealthLayby.Repositories.Repositories;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.API.Controllers
{
    /// <summary>
    /// CategoryController
    /// </summary>
    /// <seealso cref="HealthLayby.API.Controllers.BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : BaseController
    {
        #region Private members

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// The category repository
        /// </summary>
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// The string customer profile root path
        /// </summary>
        private readonly string _strCustomerProfileRootPath;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="categoryRepository">The category repository.</param>
        public CategoryController(
                                    IHttpContextAccessor httpContextAccessor,
                                    ICategoryRepository categoryRepository,
                                    IConfiguration configuration
                                 ) : base(httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _configuration = configuration;
            _strCustomerProfileRootPath = _configuration.GetValue("Urls:APIPortalURL", "");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the category list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCategoryList")]
        public async Task<IActionResult> GetCategoryList()
        {
            try
            {
                var data = await _categoryRepository.GetCategoryListAsyncForAPI();
                foreach(var u in data)
                {
                    u.Image = $"{_strCustomerProfileRootPath}/{u.Image}";
                }

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
