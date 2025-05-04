using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.ApiViewModels.CustomerPlans.Request
{
    /// <summary>
    /// Agreement Request
    /// </summary>
    public class AgreementRequest
    {
        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public long ServiceId { get; set; }
    }
}