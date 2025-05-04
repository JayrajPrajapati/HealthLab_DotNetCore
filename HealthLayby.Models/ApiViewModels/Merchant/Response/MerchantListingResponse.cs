using System.Drawing;

namespace HealthLayby.Models.ApiViewModels.Merchant.Response
{
    public class MerchantListingResponse
    {
        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public long MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the profile pic.
        /// </summary>
        /// <value>
        /// The profile pic.
        /// </value>
        public string ProfilePic { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>
        /// The logo.
        /// </value>
        public string Logo { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the clinic.
        /// </summary>
        /// <value>
        /// The name of the clinic.
        /// </value>
        public string ClinicName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        public DateTime CreatedOn { get; set; }
    }
}
