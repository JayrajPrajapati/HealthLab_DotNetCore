using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.ApiViewModels.Bank.Request
{
    /// <summary>
    /// ValidateBSBNumberRequest
    /// </summary>
    public class ValidateBSBNumberRequest
    {
        /// <summary>
        /// Gets or sets the BSB number.
        /// </summary>
        /// <value>
        /// The BSB number.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.NumberRegex, ErrorMessage = MessageConstant.BankBSBMaxLength)]
        [StringLength(maximumLength: 06, MinimumLength =06, ErrorMessage = MessageConstant.BankBSBMaxLength)]
        public string BSBNumber { get; set; } = string.Empty;
    }
}
