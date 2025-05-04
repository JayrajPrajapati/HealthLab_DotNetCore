using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.ApiViewModels.CustomerPlans.Request
{
    /// <summary>
    /// Plan Review Request Model 
    /// </summary>
    public class PlanViewRequestModel
    {
        /// <summary>
        /// Gets or sets the customer plan identifier.
        /// </summary>
        /// <value>
        /// The customer plan identifier.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public long CustomerPlanId { get; set; }
    }
}
