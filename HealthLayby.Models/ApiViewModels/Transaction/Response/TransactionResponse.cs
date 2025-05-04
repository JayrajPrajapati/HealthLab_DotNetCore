namespace HealthLayby.Models.ApiViewModels.Transaction.Response
{
    /// <summary>
    /// Transaction Response
    /// </summary>
    public class TransactionResponse
    {
        /// <summary>
        /// Gets or sets the transaction details.
        /// </summary>
        /// <value>
        /// The transaction details.
        /// </value>
        public List<TransactionDetails> TransactionDetails { get; set; } = null!;
        /// <summary>
        /// Gets or sets the transaction PDF.
        /// </summary>
        /// <value>
        /// The transaction PDF.
        /// </value>
        public string TransactionPDF { get; set; } = string.Empty;
    }


    /// <summary>
    /// Transaction Details
    /// </summary>
    public class TransactionDetails
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public long Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets the plan sku.
        /// </summary>
        /// <value>
        /// The plan sku.
        /// </value>
        public Guid PlanSku { get; set; }
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; set; }

    }
}