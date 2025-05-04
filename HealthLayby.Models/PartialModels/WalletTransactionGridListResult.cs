namespace HealthLayby.Models.Models
{
    /// <summary>
    ///   WalletTransactionGridListResult
    /// </summary>
    public partial class WalletTransactionGridListResult
    {

        /// <summary>
        ///   Gets or sets the transaction identifier.
        /// </summary>
        /// <value>
        /// The transaction identifier.
        /// </value>
        public long TransactionID { get; set; }

        /// <summary>
        ///   Gets or sets the transaction code.
        /// </summary>
        /// <value>
        /// The transaction code.
        /// </value>
        public string TransactionCode { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the customer.
        /// </summary>
        /// <value>
        /// The customer.
        /// </value>
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the paid amount.
        /// </summary>
        /// <value>
        /// The paid amount.
        /// </value>
        public decimal PaidAmount { get; set; }

        /// <summary>
        ///   Gets or sets the commission.
        /// </summary>
        /// <value>
        /// The commission.
        /// </value>
        public decimal Commission { get; set; }

        /// <summary>
        ///   Gets or sets the created date.
        /// </summary>
        /// <value>
        /// The created date.
        /// </value>
        public string CreatedDate { get; set; } = string.Empty;
    }
}
