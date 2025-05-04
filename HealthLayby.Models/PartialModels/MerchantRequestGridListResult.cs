namespace HealthLayby.Models.Models
{
    /// <summary>
    ///   Merchant Request Grid List 
    /// </summary>
    public partial class MerchantRequestGridListResult
    {

        /// <summary>
        ///   Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public long MerchantId { get; set; }

        /// <summary>
        ///   Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the name of the clinic.
        /// </summary>
        /// <value>
        /// The name of the clinic.
        /// </value>
        public string ClinicName { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string ClinicLocation { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the logo.
        /// </summary>
        /// <value>
        /// The logo.
        /// </value>
        public string Logo { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the profile pic.
        /// </summary>
        /// <value>
        /// The profile pic.
        /// </value>
        public string ProfilePic { get; set; } = string.Empty;

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
        /// The created on.
        /// </value>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the services names.
        /// </summary>
        /// <value>
        /// The services names.
        /// </value>
        public string ServicesNames { get; set; } = string.Empty;
    }
}
