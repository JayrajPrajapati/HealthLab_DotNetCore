using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.AdminViewModels
{
    /// <summary>
    /// Change Password Model
    /// </summary>
    public class ChangePasswordModel
    {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Display(Name = "Current Password")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.Password, ErrorMessage = MessageConstant.PasswordNotValid)]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = MessageConstant.PasswordMinMaxLength)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Creates new password.
        /// </summary>
        /// <value>
        /// The new password.
        /// </value>
        [Display(Name = "New Password")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.Password, ErrorMessage = MessageConstant.PasswordNotValid)]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = MessageConstant.PasswordMinMaxLength)]
        public string NewPassword { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>
        /// The confirm password.
        /// </value>
        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [Compare("NewPassword", ErrorMessage = MessageConstant.CompareNotValid)]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = MessageConstant.PasswordMinMaxLength)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
