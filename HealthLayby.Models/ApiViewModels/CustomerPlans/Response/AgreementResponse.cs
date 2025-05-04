namespace HealthLayby.Models.ApiViewModels.CustomerPlans.Response
{
    /// <summary>
    /// Agreement Response
    /// </summary>
    public class AgreementResponse
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the schedule plan transaction details.
        /// </summary>
        /// <value>
        /// The schedule plan transaction details.
        /// </value>
        public List<SchedulePlanTransactionDetails> SchedulePlanTransactionDetails { get; set; } = null!;
    }

    /// <summary>
    /// Schedule Plan Transaction Details
    /// </summary>
    public class SchedulePlanTransactionDetails
    {
        /// <summary>
        /// Gets or sets the schedule date.
        /// </summary>
        /// <value>
        /// The schedule date.
        /// </value>
        public DateTime ScheduleDate { get; set; }
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public decimal Amount { get; set; }
    }
}