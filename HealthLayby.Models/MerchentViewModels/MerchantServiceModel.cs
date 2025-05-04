namespace HealthLayby.Models.MerchentViewModels
{
    public class MerchantServiceModel
    {
        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>
        public long ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the service description.
        /// </summary>
        /// <value>
        /// The service description.
        /// </value>
        public string ServiceDescription { get; set; }

        /// <summary>
        /// Gets or sets the service image.
        /// </summary>
        /// <value>
        /// The service image.
        /// </value>
        public string ServiceImage { get; set; }

        /// <summary>
        /// Gets or sets the service created on.
        /// </summary>
        /// <value>
        /// The service created on.
        /// </value>
        public string ServiceCreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the service price.
        /// </summary>
        /// <value>
        /// The service price.
        /// </value>
        public decimal ServicePrice { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }
    }
}
