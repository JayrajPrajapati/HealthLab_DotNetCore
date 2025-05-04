using Azure.Core;
using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using log4net;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace HealthLayby.API.Infrastructure.UploadContent
{
    /// <summary>
    /// UploadContentHelper
    /// </summary>
    public class UploadContentHelper : IUploadContentHelper
    {
        #region Private Members

        /// <summary>
        /// The base URL
        /// </summary>
        private readonly string _baseURL;

        /// <summary>
        /// The secret key
        /// </summary>
        private readonly string _secretKey;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadContentHelper"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public UploadContentHelper(IConfiguration configuration)
        {
            _baseURL = configuration.GetValue("Urls:AdminPortalURL", "");
            _secretKey = configuration.GetValue("FileUploadKey", "");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the customer profile.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateCustomerProfile(IFormFile file)
        {
            try
            {
                string strFileName = "";
                HttpClient httpClient = HttpConnection.CreateClient(_baseURL, _secretKey);

                var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(file.OpenReadStream());

                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                content.Add(fileContent, "file", file.FileName);

                var response = await httpClient.PostAsync(ApiEndPointConstant.UploadCustomerProfileImage, content);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    if (responseContent is not null)
                    {
                        strFileName = JsonConvert.DeserializeObject<string>(responseContent);
                    }
                    return (true, strFileName);
                }
                return (false, ApiMessages.InternalServerError);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.APILogError
                (
                    logger: _logger,
                    hostUrl: "",
                    exception: ex,
                    jwtToken: null,
                    requestData: JsonConvert.SerializeObject(file.FileName)
                );
                return (false, ApiMessages.InternalServerError);
                //throw;
            }
        }

        #endregion
    }
}
