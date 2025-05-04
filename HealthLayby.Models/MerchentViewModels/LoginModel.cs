using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.MerchentViewModels
{
    public class LoginModel
    {
        /// <summary>
        ///   Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.EmailRegex, ErrorMessage = MessageConstant.NotValid)]
        [MaxLength(LengthConstant.EmailMaxLength, ErrorMessage = MessageConstant.EmailAddressMaxLength)]
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Display(Name = "Password")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.Password, ErrorMessage = MessageConstant.PasswordNotValid)]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = MessageConstant.PasswordMinMaxLength)]
        public string Password { get; set; } = string.Empty;
    }
}
