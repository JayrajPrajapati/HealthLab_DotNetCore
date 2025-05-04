namespace HealthLayby.Models.Models
{
    /// <summary>
    ///   Rewards Grid List Result
    /// </summary>
    public partial class RewardsGridListResult
    {
        /// <summary>
        ///   Gets or sets the reward identifier.
        /// </summary>
        /// <value>
        ///   The reward identifier.
        /// </value>
        public long RewardId { get; set; }

        /// <summary>
        ///   Gets or sets the month.
        /// </summary>
        /// <value>
        ///   The month.
        /// </value>
        public int Month { get; set; }

        /// <summary>
        ///   Gets or sets the discount.
        /// </summary>
        /// <value>
        ///   The discount.
        /// </value>
        public string Discount { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the name of the category.
        /// </summary>
        /// <value>
        ///   The name of the category.
        /// </value>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the full name.
        /// </summary>
        /// <value>
        ///   The full name.
        /// </value>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>Gets or sets the profile pic.</summary>
        /// <value>The profile pic.</value>
        public string ProfilePic { get; set; } = string.Empty;
    }
}
