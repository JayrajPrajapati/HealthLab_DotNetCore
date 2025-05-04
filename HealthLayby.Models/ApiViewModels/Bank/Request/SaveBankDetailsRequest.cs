using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.ApiViewModels.Bank.Request
{
    /// <summary>
    /// SaveBanDetailsRequest
    /// </summary>
    public class SaveBankDetailsRequest
    {
        /// <summary>
        /// Gets or sets the BSB number.
        /// </summary>
        /// <value>
        /// The BSB number.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.NumberRegex, ErrorMessage = MessageConstant.BankBSBMaxLength)]
        [StringLength(maximumLength: 06, MinimumLength = 06, ErrorMessage = MessageConstant.BankBSBMaxLength)]
        public string BSBNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the bank.
        /// </summary>
        /// <value>
        /// The name of the bank.
        /// </value>
        public string BankName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the bank location.
        /// </summary>
        /// <value>
        /// The bank location.
        /// </value>
        public string BankLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [StringLength(LengthConstant.NameMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        /// <value>
        /// The account number.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [StringLength(maximumLength: 10, MinimumLength = 7,ErrorMessage = MessageConstant.NotValid)]
        public string AccountNumber { get; set; } = string.Empty;
    }
}
