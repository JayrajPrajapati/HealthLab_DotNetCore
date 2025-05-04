using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthLayby.Models.AdminViewModels
{
    /// <summary>
    /// Lay By Model
    /// </summary>
    public class LayByModel
    {
        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>
        /// The logo.
        /// </value>
        public string MerchantImage { get; set; }
        /// <summary>
        /// Gets or sets the name of the merchant.
        /// </summary>
        /// <value>
        /// The name of the merchant.
        /// </value>
        public string MerchantName { get; set; }
        /// <summary>
        /// Gets or sets the merchant email.
        /// </summary>
        /// <value>
        /// The merchant email.
        /// </value>
        public string MerchantEmail { get; set; }
        /// <summary>
        /// Gets or sets the merchant phone number.
        /// </summary>
        /// <value>
        /// The merchant phone number.
        /// </value>
        public string MerchantPhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the name of the customer.
        /// </summary>
        /// <value>
        /// The name of the customer.
        /// </value>
        public string CustomerName { get; set; }
        /// <summary>
        /// Gets or sets the customer email.
        /// </summary>
        /// <value>
        /// The customer email.
        /// </value>
        public string CustomerEmail { get; set; }
        /// <summary>
        /// Gets or sets the customer phone number.
        /// </summary>
        /// <value>
        /// The customer phone number.
        /// </value>
        public string CustomerPhoneNumber { get; set; }
        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the category image.
        /// </summary>
        /// <value>
        /// The category image.
        /// </value>
        public string CategoryImage { get; set; }
        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; }
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public string Duration { get; set; }
        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public string Frequency { get; set; }
        /// <summary>
        /// Gets or sets the total amount received.
        /// </summary>
        /// <value>
        /// The total amount received.
        /// </value>
        public decimal TotalAmountReceived { get; set; }
        /// <summary>
        /// Gets or sets the next deduction date.
        /// </summary>
        /// <value>
        /// The next deduction date.
        /// </value>
        public DateTime? NextDeductionDate { get; set; }
        /// <summary>
        /// Gets or sets the next amount deduction.
        /// </summary>
        /// <value>
        /// The next amount deduction.
        /// </value>
        public decimal NextAmountDeduction { get; set; }

        /// <summary>
        /// Gets or sets the pay amount.
        /// </summary>
        /// <value>
        /// The pay amount.
        /// </value>
        public decimal PayAmount { get; set; }
    }
}
