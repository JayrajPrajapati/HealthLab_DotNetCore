using HealthLayby.Models.Models;

namespace HealthLayby.Models.MerchentViewModels
{
    /// <summary>
    /// DashboardMerchantModel
    /// </summary>
    public class DashboardMerchantModel
    {
        /// <summary>
        /// Gets or sets the total amount received count.
        /// </summary>
        /// <value>
        /// The total amount received count.
        /// </value>
        public int TotalAmountReceivedCount { get; set; }

        /// <summary>
        /// Gets or sets the amount service category count.
        /// </summary>
        /// <value>
        /// The amount service category count.
        /// </value>
        public int AmountServiceCategoryCount { get; set; }

        /// <summary>
        /// Gets or sets the total earning count.
        /// </summary>
        /// <value>
        /// The total earning count.
        /// </value>
        public int TotalEarningCount { get; set; }

        /// <summary>
        /// Gets or sets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        public List<Service> Service { get; set; }
    }
}
