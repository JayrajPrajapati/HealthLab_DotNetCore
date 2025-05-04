namespace HealthLayby.Models.ApiViewModels.CustomerPlans.Response
{
    /// <summary>
    /// Plan Review Response
    /// </summary>
    public class PlanReviewResponse
    {
        /// <summary>
        /// Gets or sets the name of the plan.
        /// </summary>
        /// <value>
        /// The name of the plan.
        /// </value>
        public string PlanName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the plan image.
        /// </summary>
        /// <value>
        /// The plan image.
        /// </value>
        public string PlanImage { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the merchant.
        /// </summary>
        /// <value>
        /// The merchant.
        /// </value>
        public string Merchant { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public string Services { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the family members.
        /// </summary>
        /// <value>
        /// The family members.
        /// </value>
        public string FamilyMembers { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public string Frequency { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the schedule infos.
        /// </summary>
        /// <value>
        /// The schedule infos.
        /// </value>
        public List<ScheduleInfo> ScheduleInfos { get; set; } = null!;

        /// <summary>
        /// Gets or sets the progress statics.
        /// </summary>
        /// <value>
        /// The progress statics.
        /// </value>
        public decimal ProgressStatics { get; set; }
    }

    public class ScheduleInfo
    {
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; set; }
        /// <summary>
        /// Gets or sets the schedule date.
        /// </summary>
        /// <value>
        /// The schedule date.
        /// </value>
        public DateTime ScheduleDate { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; } = string.Empty;
    }
}