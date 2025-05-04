namespace HealthLayby.Models.ApiViewModels.Bank.Response
{
    /// <summary>
    /// ValidateBSBNumberResponse
    /// </summary>
    public class ValidateBSBNumberResponse
    {
        /// <summary>
        /// Gets or sets the bank BSB directory identifier.
        /// </summary>
        /// <value>
        /// The bank BSB directory identifier.
        /// </value>
        public int BankBSBDirectoryId { get; set; }
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
    }
}
