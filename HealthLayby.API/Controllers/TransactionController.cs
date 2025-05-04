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
    /// Transaction Controller
    /// </summary>
    /// <seealso cref="HealthLayby.API.Controllers.BaseController" />
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : BaseController
    {
        #region Private Members
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// The service repository
        /// </summary>
        private readonly ITransactionRepository _transactionRepository;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionController"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="transactionRepository">The transaction repository.</param>
        public TransactionController(IHttpContextAccessor httpContextAccessor, ITransactionRepository transactionRepository) : base(httpContextAccessor)
        {
            _transactionRepository = transactionRepository;
        }
        #endregion

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("GetTransaction")]
        public async Task<IActionResult> GetTransaction()
        {
            try
            {
                var (isSuccess, message, ServiceListingResponse) = await _transactionRepository.GetTransactionList();

                if (isSuccess)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                                     (
                                         Status: StatusCodes.Status200OK,
                                         Message: message,
                                         Data: ServiceListingResponse
                                     ));
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
                                     (
                                         Status: StatusCodes.Status400BadRequest,
                                         Message: message
                                     ));
                }

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

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }
    }
}
