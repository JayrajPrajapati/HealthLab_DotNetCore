using log4net;
using System.Text;

namespace HealthLayby.Helpers.CommonMethod
{
    /// <summary>
    /// ErrorLogHelper
    /// </summary>
    public static class ErrorLogHelper
    {
        /// <summary>
        /// APIs the log error.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="hostUrl">The host URL.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="jwtToken">The JWT token.</param>
        /// <param name="requestData">The request data.</param>
        public static void APILogError(ILog logger, 
                                       string hostUrl,                                                                          
                                       Exception exception,
                                       string? jwtToken = null,
                                       string? requestData = null)
        {
            var error = new StringBuilder();
            error.AppendLine($"==========Start==========");
            error.AppendLine($"Date: {DateTime.Now}");
            error.AppendLine($"URL: {hostUrl}");

            if (!string.IsNullOrWhiteSpace(requestData))
            {
                error.AppendLine($"Request Data: {requestData}");
            }            

            if (!string.IsNullOrWhiteSpace(jwtToken))
            {
                error.AppendLine($"Token: {jwtToken}");
            }
            
            error.AppendLine($"Message: {exception.Message}");

            if (!string.IsNullOrWhiteSpace(exception.StackTrace))
            {
                error.AppendLine("StackTrace: " + exception.StackTrace);
            }
            
            error.AppendLine($"===========End===========");

            logger.Error(error);
        }
    }
}
