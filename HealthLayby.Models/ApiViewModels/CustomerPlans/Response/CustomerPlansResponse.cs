namespace HealthLayby.Models.ApiViewModels.CustomerPlans.Response
{
    /// <summary>
    /// CustomerPlansResponse
    /// </summary>
    public class CustomerPlansResponse
    {
        /// <summary>
        /// Gets or sets the customer plan identifier.
        /// </summary>
        /// <value>
        /// The customer plan identifier.
        /// </value>
        public long CustomerPlanId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public long CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the master service.
        /// </summary>
        /// <value>
        /// The name of the master service.
        /// </value>
        public string MasterServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the merchant.
        /// </summary>
        /// <value>
        /// The name of the merchant.
        /// </value>
        public string MerchantName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the plan image.
        /// </summary>
        /// <value>
        /// The plan image.
        /// </value>
        public string PlanImage { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the name of the plan.
        /// </summary>
        /// <value>
        /// The name of the plan.
        /// </value>
        public string PlanName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime? EndDate { get; set; }
    }
}
