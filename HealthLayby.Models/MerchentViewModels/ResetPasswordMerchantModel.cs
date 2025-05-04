using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.MerchentViewModels
{
    /// <summary>
    ///   Reset Password Model
    /// </summary>
    public class ResetPasswordMerchantModel
    {
        /// <summary>
        ///   Gets or sets the reset token.
        /// </summary>
        /// <value>
        /// The reset token.
        /// </value>
        [Required(ErrorMessage = MessageConstant.InValidResetToken)]
        public string ResetToken { get; set; } = string.Empty;

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

        /// <summary>
        ///   Gets or sets the confirm password.
        /// </summary>
        /// <value>
        /// The confirm password.
        /// </value>
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = MessageConstant.CompareNotValid)]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.Password, ErrorMessage = MessageConstant.PasswordNotValid)]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = MessageConstant.PasswordMinMaxLength)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
