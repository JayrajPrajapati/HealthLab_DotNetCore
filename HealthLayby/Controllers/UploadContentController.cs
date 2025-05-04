using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// UploadContentController
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class UploadContentController : Controller
    {
        #region Private variables

        /// <summary>
        /// The env
        /// </summary>
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// The file upload key
        /// </summary>
        private readonly string _fileUploadKey;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadContentController" /> class.
        /// </summary>
        /// <param name="env">The env.</param>
        /// <param name="configuration">The configuration.</param>
        public UploadContentController(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _fileUploadKey = configuration.GetValue("FileUploadKey", string.Empty) ?? string.Empty;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Uploads the customer profile image.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadCustomerProfileImage")]
        public IActionResult UploadCustomerProfileImage(IFormFile file)
        {
            if (Request.Headers.TryGetValue("Authorization", out var token) && token.ToString() != _fileUploadKey)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, MessageConstant.InvalidAuthorization);
            }

            #region File Upload

            if (file is null)
            {
                file = Request.Form.Files[0];
            }

            if (file is not null)
            {
                string strFileExtension = Path.GetExtension(file.FileName);

                if (!new string[] { ".png", ".jpg", ".jpeg" }.Any(s => s == strFileExtension.ToLower()))
                {
                    return StatusCode(StatusCodes.Status415UnsupportedMediaType, MessageConstant.UnsupportedMediaTypes);
                }
                //FileUploadHelper.DeleteFile
                //(
                //    path: Path.Combine(_env.WebRootPath, DirectoryConstant.MerchantProfilePicDirectory, merchant.ProfilePic)
                //);

                string strFileName = FileUploadHelper.UploadFile
                (
                    file: file,
                    extension: strFileExtension,
                    path: Path.Combine(_env.WebRootPath, DirectoryConstant.CustomerProfilePicDirectory)
                );

                return StatusCode(StatusCodes.Status200OK, strFileName);
            }

            #endregion

            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstant.InternalServerError);
        }

        /// <summary>
        /// Deletes the customer profile image.
        /// </summary>
        /// <param name="imageName">Name of the image.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteCustomerProfileImage")]
        public IActionResult DeleteCustomerProfileImage(string imageName)
        {
            if (Request.Headers.TryGetValue("Authorization", out var token) && token.ToString() != _fileUploadKey)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, MessageConstant.InvalidAuthorization);
            }

            #region File Upload

            if (imageName is not null)
            {
                string strFileExtension = Path.GetExtension(imageName);

                if (!new string[] { ".png", ".jpg", ".jpeg" }.Any(s => s == strFileExtension.ToLower()))
                {
                    return StatusCode(StatusCodes.Status415UnsupportedMediaType, MessageConstant.UnsupportedMediaTypes);
                }

                FileUploadHelper.DeleteFile
                (
                    path: Path.Combine(_env.WebRootPath, DirectoryConstant.CustomerProfilePicDirectory, imageName)
                );

                return StatusCode(StatusCodes.Status200OK, MessageConstant.FileDeletedSuccessfully);
            }

            #endregion

            return StatusCode(StatusCodes.Status500InternalServerError, MessageConstant.InternalServerError);
        }

        #endregion
    }
}
