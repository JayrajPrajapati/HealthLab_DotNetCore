using HealthLayby.API.Infrastructure.UploadContent;
using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels;
using HealthLayby.Models.ApiViewModels.Customer.Request;
using HealthLayby.Repositories.Repositories;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HealthLayby.API.Controllers
{
    /// <summary>
    /// CustomerController
    /// </summary>
    /// <seealso cref="HealthLayby.API.Controllers.BaseController" />
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CustomerController : BaseController
    {

        #region Private members

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        /// <summary>
        /// The customer repository
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// The configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// The string customer profile root path
        /// </summary>
        private readonly string _strCustomerProfileRootPath;

        /// <summary>
        /// The upload content helper
        /// </summary>
        IUploadContentHelper _uploadContentHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="customerRepository">The customer repository.</param>
        /// <param name="configuration">The configuration.</param>
        public CustomerController(
                                    IHttpContextAccessor httpContextAccessor,
                                    ICustomerRepository customerRepository,
                                    IConfiguration configuration,
                                    IUploadContentHelper uploadContentHelper
                                 ) : base(httpContextAccessor)
        {
            _customerRepository = customerRepository;
            _configuration = configuration;
            _uploadContentHelper = uploadContentHelper;
            _strCustomerProfileRootPath = _configuration.GetValue("Urls:AdminPortalURL", "");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the profile details.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProfileDetails")]
        public async Task<IActionResult> GetProfileDetails()
        {
            try
            {
                var data = await _customerRepository.GetProfileDetails(claim.CustomerId);

                if (data is null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status500InternalServerError,
                        Message: ApiMessages.InternalServerError
                    ));
                }

                data.ProfilePicPath = $"{_strCustomerProfileRootPath}{DirectoryConstant.CustomerProfilePicDirectory}/{data.ProfilePic}";

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
        /// Updates the profile details.
        /// </summary>
        /// <param name="updateProfileDetails">The update profile details.</param>
        /// <param name="profileImage">The profile image.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        //[NonAction]
        [HttpPost]
        [Route("UpdateProfileDetails")]
        public async Task<IActionResult> UpdateProfileDetails([FromForm] UpdateProfileDetailsRequest updateProfileDetailsRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.InvalidRequestPayload,
                        Data: ModelState
                    ));
                }

                updateProfileDetailsRequest.ProfilePic = null;

                if (updateProfileDetailsRequest.ProfileImage is not null)
                {
                    string strFileExtension = Path.GetExtension(updateProfileDetailsRequest.ProfileImage.FileName);

                    if (!new string[] { ".png", ".jpg", ".jpeg" }.Any(s => s == strFileExtension.ToLower()))
                    {
                        return StatusCode(StatusCodes.Status200OK, new ResponseModel
                        (
                            Status: StatusCodes.Status400BadRequest,
                            Message: ApiMessages.UnsupportedMediaTypes,
                            Data: ModelState
                        ));
                    }

                    var (fileUploadStatus, fileUploadName) = await _uploadContentHelper.UpdateCustomerProfile(file: updateProfileDetailsRequest.ProfileImage);

                    if (fileUploadStatus)
                    {
                        updateProfileDetailsRequest.ProfilePic = fileUploadName;
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new ResponseModel
                                        (
                                            Status: StatusCodes.Status400BadRequest,
                                            Message: ApiMessages.ErrorUploadingProfilePic
                                        ));

                    }
                }

                var (status, message) = await _customerRepository.UpdateProfileDetails(claim.CustomerId, updateProfileDetailsRequest);

                if (status)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status200OK,
                        Message: message
                    ));
                }

                throw new Exception(ApiMessages.ProfileNotUpdated);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.APILogError
                (
                    logger: _logger,
                    hostUrl: $"{Request.Scheme}://{Request.Host.Value}{Request.Path}",
                    exception: ex,
                    jwtToken: Request.Headers.TryGetValue("Authorization", out var token) ? token.ToString() : null,
                    requestData: JsonConvert.SerializeObject(updateProfileDetailsRequest)
                );

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }

        /// <summary>
        /// Saves the family member.
        /// </summary>
        /// <param name="saveFamilyMemberRequest">The save family member request.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [Route("SaveFamilyMember")]
        public async Task<IActionResult> SaveFamilyMember([FromBody] SaveFamilyMemberRequest saveFamilyMemberRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.InvalidRequestPayload,
                        Data: ModelState
                    ));
                }

                var (status, message) = await _customerRepository.SaveFamilyMember(saveFamilyMemberRequest, claim.CustomerId);

                if (status)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status200OK,
                        Message: message
                    ));
                }

                throw new Exception(ApiMessages.FamilyMemberNotSaved);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.APILogError
                (
                    logger: _logger,
                    hostUrl: $"{Request.Scheme}://{Request.Host.Value}{Request.Path}",
                    exception: ex,
                    jwtToken: Request.Headers.TryGetValue("Authorization", out var token) ? token.ToString() : null,
                    requestData: JsonConvert.SerializeObject(saveFamilyMemberRequest)
                );

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.FamilyMemberNotSaved
                ));
            }
        }

        /// <summary>
        /// Deletes the family member.
        /// </summary>
        /// <param name="familyMemberId">The family member identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpDelete]
        [Route("DeleteFamilyMember")]
        public async Task<IActionResult> DeleteFamilyMember(long familyMemberId)
        {
            try
            {
                var (status, message) = await _customerRepository.DeleteFamilyMember(familyMemberId);

                if (status)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                   (
                       Status: StatusCodes.Status200OK,
                       Message: message
                   ));
                }

                throw new Exception(ApiMessages.FamilyMemberNotDeleted);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.APILogError
                (
                    logger: _logger,
                    hostUrl: $"{Request.Scheme}://{Request.Host.Value}{Request.Path}",
                    exception: ex,
                    jwtToken: Request.Headers.TryGetValue("Authorization", out var token) ? token.ToString() : null,
                    requestData: JsonConvert.SerializeObject(familyMemberId)
                );

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }

        [HttpGet]
        [Route("GetEmergencyContacts")]
        public async Task<IActionResult> GetEmergencyContacts()
        {
            try
            {
                var data = await _customerRepository.GetEmergencyContacts(claim.CustomerId);

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
        /// Saves the emergency contact.
        /// </summary>
        /// <param name="saveEmergencyContactRequest">The save emergency contact request.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        [Route("SaveEmergencyContact")]
        public async Task<IActionResult> SaveEmergencyContact([FromBody] SaveEmergencyContactRequest saveEmergencyContactRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status400BadRequest,
                        Message: ApiMessages.InvalidRequestPayload,
                        Data: ModelState
                    ));
                }

                var (status, message) = await _customerRepository.SaveEmergencyContact(saveEmergencyContactRequest, claim.CustomerId);

                if (status)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status200OK,
                        Message: message
                    ));
                }

                throw new Exception(ApiMessages.EmergencyContactNotSaved);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.APILogError
                (
                    logger: _logger,
                    hostUrl: $"{Request.Scheme}://{Request.Host.Value}{Request.Path}",
                    exception: ex,
                    jwtToken: Request.Headers.TryGetValue("Authorization", out var token) ? token.ToString() : null,
                    requestData: JsonConvert.SerializeObject(saveEmergencyContactRequest)
                );

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }

        /// <summary>
        /// Deletes the emergency contact.
        /// </summary>
        /// <param name="emergencyContactId">The emergency contact identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpDelete]
        [Route("DeleteEmergencyContact")]
        public async Task<IActionResult> DeleteEmergencyContact(long emergencyContactId)
        {
            try
            {
                var (status, message) = await _customerRepository.DeleteEmergencyContact(emergencyContactId);

                if (status)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status200OK,
                        Message: message
                    ));
                }

                throw new Exception(ApiMessages.EmergencyContactNotDeleted);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.APILogError
                (
                    logger: _logger,
                    hostUrl: $"{Request.Scheme}://{Request.Host.Value}{Request.Path}",
                    exception: ex,
                    jwtToken: Request.Headers.TryGetValue("Authorization", out var token) ? token.ToString() : null,
                    requestData: JsonConvert.SerializeObject(emergencyContactId)
                );

                return StatusCode(StatusCodes.Status200OK, new ResponseModel
                (
                    Status: StatusCodes.Status500InternalServerError,
                    Message: ApiMessages.InternalServerError
                ));
            }
        }

        /// <summary>
        /// Deletes the customer account.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        [HttpDelete]
        [Route("DeleteAccount")]
        public async Task<IActionResult> DeleteCustomerAccount()
        {
            try
            {
                var (status, message) = await _customerRepository.DeleteCustomerAccount(claim.CustomerId);

                if (status)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseModel
                    (
                        Status: StatusCodes.Status200OK,
                        Message: message
                    ));
                }

                throw new Exception(ApiMessages.ProfileNotDeleted);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.APILogError
                (
                    logger: _logger,
                    hostUrl: $"{Request.Scheme}://{Request.Host.Value}{Request.Path}",
                    exception: ex,
                    jwtToken: Request.Headers.TryGetValue("Authorization", out var token) ? token.ToString() : null,
                    requestData: JsonConvert.SerializeObject(claim.CustomerId)
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
