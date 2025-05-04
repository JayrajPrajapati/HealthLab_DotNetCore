namespace HealthLayby.Models.StripeModels
{
    /// <summary>
    /// AddStripePayment
    /// </summary>
    public class AddStripePayment
    {
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public string CustomerId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the receipt email.
        /// </summary>
        /// <value>
        /// The receipt email.
        /// </value>
        public string ReceiptEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public long Amount { get; set; }
    }
}
