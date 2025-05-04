using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.MerchentViewModels
{
    /// <summary>
    ///   ForgotPasswordModel
    /// </summary>
    public class ForgotPasswordMerchantModel
    {

        /// <summary>
        ///   Gets or sets the email address.
        /// </summary>
        /// <value>
        ///   The email address.
        /// </value>
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.EmailRegex, ErrorMessage = MessageConstant.NotValid)]
        [MaxLength(LengthConstant.EmailMaxLength, ErrorMessage = MessageConstant.EmailAddressMaxLength)]
        public string EmailAddress { get; set; } = string.Empty;
    }
}
