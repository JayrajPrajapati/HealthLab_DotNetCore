namespace HealthLayby.Models.ApiViewModels.Service.Response
{
    public class ServiceDetailsResponse
    {
        /// <summary>
        /// Gets or sets the service details identifier.
        /// </summary>
        /// <value>
        /// The service details identifier.
        /// </value>
        public long ServiceDetailsId { get; set; }

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
        /// Gets or sets the service price.
        /// </summary>
        /// <value>
        /// The service price.
        /// </value>
        public decimal ServicePrice { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public long Duration { get; set; }
        /// <summary>
        /// Gets or sets the weeks.
        /// </summary>
        /// <value>
        /// The weeks.
        /// </value>
        public long Weeks { get; set; }

        /// <summary>
        /// Gets or sets the payment frequent.
        /// </summary>
        /// <value>
        /// The payment frequent.
        /// </value>
        public string PaymentFrequent { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the pay amount.
        /// </summary>
        /// <value>
        /// The pay amount.
        /// </value>
        public decimal PayAmount { get; set; }

        /// <summary>
        /// Gets or sets the name of the merchent.
        /// </summary>
        /// <value>
        /// The name of the merchent.
        /// </value>
        public string MerchentName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the family member.
        /// </summary>
        /// <value>
        /// The family member.
        /// </value>
        public string FamilyMember { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string MerchantImage { get; set; } = string.Empty;
    }
}