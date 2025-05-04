namespace HealthLayby.Models.ApiViewModels.Service.Response
{
    /// <summary>
    /// ServiceListingResponse
    /// </summary>
    public class ServiceListingResponse
    {
        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>
        public long ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the service image.
        /// </summary>
        /// <value>
        /// The service image.
        /// </value>
        public string ServiceImage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public decimal Price { get; set; }
    }
}
