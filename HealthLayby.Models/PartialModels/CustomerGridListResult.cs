namespace HealthLayby.Models.Models
{
    /// <summary>
    ///   Customer Grid List Result
    /// </summary>
    public partial class CustomerGridListResult
    {
        /// <summary>
        ///   Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        ///   The customer identifier.
        /// </value>
        public long CustomerId { get; set; }

        /// <summary>
        ///   Gets or sets the full name.
        /// </summary>
        /// <value>
        ///   The full name.
        /// </value>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the phone number.
        /// </summary>
        /// <value>
        ///   The phone number.
        /// </value>
        public string? PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the emergency contact number.
        /// </summary>
        /// <value>
        ///   The emergency contact number.
        /// </value>
        public string? EmergencyContactNumber { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the total plans.
        /// </summary>
        /// <value>
        ///   The total plans.
        /// </value>
        public int TotalPlans { get; set; }

        /// <summary>
        ///   Gets or sets the wallet amount.
        /// </summary>
        /// <value>
        ///   The wallet amount.
        /// </value>
        public decimal WalletAmount { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        ///   Gets or sets the created on.
        /// </summary>
        /// <value>
        ///   The created on.
        /// </value>
        public DateTime CreatedOn { get; set; }
    }
}
