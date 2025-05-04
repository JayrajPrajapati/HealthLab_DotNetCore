namespace HealthLayby.Models.AdminViewModels
{
    /// <summary>
    /// Dashboard Model
    /// </summary>
    public class DashboardModel
    {
        /// <summary>
        /// Gets or sets the customer count.
        /// </summary>
        /// <value>
        /// The customer count.
        /// </value>
        public int CustomerCount { get; set; }

        /// <summary>
        /// Gets or sets the merchant count.
        /// </summary>
        /// <value>
        /// The merchant count.
        /// </value>
        public int MerchantCount { get; set; }

        /// <summary>
        /// Gets or sets the service count.
        /// </summary>
        /// <value>
        /// The service count.
        /// </value>
        public int ServiceCount { get; set; }

        /// <summary>
        /// Gets or sets the customer chart data.
        /// </summary>
        /// <value>
        /// The customer chart data.
        /// </value>
        public Dictionary<string, int>? CustomerChartData { get; set; }

        /// <summary>
        /// Gets or sets the merchant chart data.
        /// </summary>
        /// <value>
        /// The merchant chart data.
        /// </value>
        public Dictionary<string, int>? MerchantChartData { get; set; }
    }
}
