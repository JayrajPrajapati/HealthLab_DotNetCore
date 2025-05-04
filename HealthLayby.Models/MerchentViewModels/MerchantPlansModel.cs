namespace HealthLayby.Models.MerchentViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class MerchantPlansModel
    {
        /// <summary>
        /// Gets or sets the customer plan identifier.
        /// </summary>
        /// <value>
        /// The customer plan identifier.
        /// </value>
        public long CustomerPlanId { get; set; }

        /// <summary>
        /// Gets or sets the name of the customer.
        /// </summary>
        /// <value>
        /// The name of the customer.
        /// </value>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets the customer phone number.
        /// </summary>
        /// <value>
        /// The customer phone number.
        /// </value>
        public string CustomerPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the customer email.
        /// </summary>
        /// <value>
        /// The customer email.
        /// </value>
        public string CustomerEmail { get; set; }

        /// <summary>
        /// Gets or sets the customer status.
        /// </summary>
        /// <value>
        /// The customer status.
        /// </value>
        public string CustomerStatus { get; set; }

        /// <summary>
        /// Gets or sets the name of the merchant.
        /// </summary>
        /// <value>
        /// The name of the merchant.
        /// </value>
        public string MerchantName { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the service amount.
        /// </summary>
        /// <value>
        /// The service amount.
        /// </value>
        public decimal ServiceAmount { get; set; }

        /// <summary>
        /// Gets or sets the name of the service category.
        /// </summary>
        /// <value>
        /// The name of the service category.
        /// </value>
        public string ServiceCategoryName { get; set; }

        /// <summary>
        /// Gets or sets the service duratin.
        /// </summary>
        /// <value>
        /// The service duratin.
        /// </value>
        public string ServiceDuration { get; set; }

        /// <summary>
        /// Gets or sets the service frequency.
        /// </summary>
        /// <value>
        /// The service frequency.
        /// </value>
        public string ServiceFrequency { get; set; }

        /// <summary>
        /// Gets or sets the plan status.
        /// </summary>
        /// <value>
        /// The plan status.
        /// </value>
        public string PlanStatus { get; set; }

        /// <summary>
        /// Gets or sets the customer total amt paid.
        /// </summary>
        /// <value>
        /// The customer total amt paid.
        /// </value>
        public decimal CustomerTotalAmtPaid { get; set;  }

        /// <summary>
        /// Gets or sets the customer next deduction date.
        /// </summary>
        /// <value>
        /// The customer next deduction date.
        /// </value>
        public string CustomerNextDeductionDate { get; set; }

        /// <summary>
        /// Gets or sets the customer next amt.
        /// </summary>
        /// <value>
        /// The customer next amt.
        /// </value>
        public decimal CustomerNextAmt { get; set; }

    }
}
