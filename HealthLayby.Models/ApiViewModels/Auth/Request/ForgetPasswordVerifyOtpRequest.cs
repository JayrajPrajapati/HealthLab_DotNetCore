using HealthLayby.Helpers.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthLayby.Models.ApiViewModels.Auth.Request
{
    /// <summary>
    /// ForgetPasswordVerifyOtpRequest
    /// </summary>
    public class ForgetPasswordVerifyOtpRequest
    {
        /// <summary>
        /// Gets or sets the otp.
        /// </summary>
        /// <value>
        /// The otp.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string OTP { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string Key { get; set; } = string.Empty;
    }
}
