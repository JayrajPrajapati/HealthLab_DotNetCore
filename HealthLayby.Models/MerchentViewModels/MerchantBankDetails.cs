using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.MerchentViewModels
{
    /// <summary>
    /// MerchantBankDetails
    /// </summary>
    public class MerchantBankDetails
    {
        /// <summary>
        /// Gets or sets the BSB number.
        /// </summary>
        /// <value>
        /// The BSB number.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string BSBNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the bank.
        /// </summary>
        /// <value>
        /// The bank.
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
        /// Gets or sets the bank account number.
        /// </summary>
        /// <value>
        /// The bank account number.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [StringLength(maximumLength: 10, MinimumLength = 7, ErrorMessage = MessageConstant.NotValid)]
        public string BankAccountNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public long merchantId { get; set; }

    }
}
