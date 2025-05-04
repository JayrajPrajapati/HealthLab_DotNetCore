using HealthLayby.Helpers.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthLayby.Models.ApiViewModels.Auth.Request
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = MessageConstant.Required)]
        public string Key { get; set; } = string.Empty;

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
