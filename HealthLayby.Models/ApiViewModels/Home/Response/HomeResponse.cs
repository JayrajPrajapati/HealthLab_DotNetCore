namespace HealthLayby.Models.ApiViewModels.Home.Response
{
    public class HomeResponse
    {

        /// <summary>
        /// Gets or sets the plan identifier.
        /// </summary>
        /// <value>
        /// The plan identifier.
        /// </value>
        public long PlanId { get; set; }
        /// <summary>
        /// Gets or sets the name of the plan.
        /// </summary>
        /// <value>
        /// The name of the plan.
        /// </value>
        public string PlanName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the plan category.
        /// </summary>
        /// <value>
        /// The plan category.
        /// </value>
        public string PlanCategoryName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the name of the plan service.
        /// </summary>
        /// <value>
        /// The name of the plan service.
        /// </value>
        public string PlanServiceName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the plan end date.
        /// </summary>
        /// <value>
        /// The plan end date.
        /// </value>
        public DateTime? PlanEndDate { get; set; }

        /// <summary>
        /// Gets or sets the plan image.
        /// </summary>
        /// <value>
        /// The plan image.
        /// </value>
        public string PlanImage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is plan active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is plan active; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlanActive { get; set; }
    }
}
