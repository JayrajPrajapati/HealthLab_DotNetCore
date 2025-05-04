using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.ApiViewModels.Registration.Request
{
    /// <summary>
    /// SignUpRequest
    /// </summary>
    public class SignUpRequest
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Display(Name = "First Name")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.Name, ErrorMessage = MessageConstant.NotValid)]
        [StringLength(maximumLength: 50, ErrorMessage = MessageConstant.NameMaxLength)]
        public string FirstName { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.Name, ErrorMessage = MessageConstant.PasswordNotValid)]
        [StringLength(maximumLength: 50, ErrorMessage = MessageConstant.NameMaxLength)]
        public string LastName { get; set; } = string.Empty;

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

        [Compare(nameof(Password), ErrorMessage = MessageConstant.CompareNotValid)]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.Password, ErrorMessage = MessageConstant.PasswordNotValid)]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = MessageConstant.PasswordMinMaxLength)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
