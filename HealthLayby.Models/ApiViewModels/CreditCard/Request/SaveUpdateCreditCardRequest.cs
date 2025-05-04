using HealthLayby.Helpers.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthLayby.Models.ApiViewModels.CreditCard.Request
{
    /// <summary>
    /// SaveUpdateCreditCardRequest
    /// </summary>
    public class SaveUpdateCreditCardRequest
    {
        /// <summary>
        /// Gets or sets the card number.
        /// </summary>
        /// <value>
        /// The card number.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.CardNumberRegex, ErrorMessage = MessageConstant.CardNumberMaxLength)]
        [StringLength(maximumLength: 16, MinimumLength = 16, ErrorMessage = MessageConstant.CardNumberMaxLength)]
        public string CardNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name on card.
        /// </summary>
        /// <value>
        /// The name on card.
        /// </value>
        public string NameOnCard { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.MonthYearRegex, ErrorMessage = MessageConstant.MonthYearMaxLength)]
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.MonthYearRegex, ErrorMessage = MessageConstant.MonthYearMaxLength)]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the CCV.
        /// </summary>
        /// <value>
        /// The CCV.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.CVVRegex, ErrorMessage = MessageConstant.CVVMaxLength)]
        [StringLength(maximumLength: 3, ErrorMessage = MessageConstant.NotValid)]
        public string CCV { get; set; } = string.Empty;
    }
}