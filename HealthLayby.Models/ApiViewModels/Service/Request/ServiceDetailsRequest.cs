using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.ApiViewModels.Service.Request
{
    /// <summary>
    /// ServiceDetailsRequest
    /// </summary>
    public class ServiceDetailsRequest
    {
        /// <summary>
        /// Gets or sets the service detail identifier.
        /// </summary>
        /// <value>
        /// The service detail identifier.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public long ServiceDetailId { get; set; }

        /// <summary>
        /// Gets or sets the merchent identifier.
        /// </summary>
        /// <value>
        /// The merchent identifier.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public long MerchentId { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the final price.
        /// </summary>
        /// <value>
        /// The final price.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public decimal PayAmount { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public long Duration { get; set; }

        /// <summary>
        /// Gets or sets the weeks.
        /// </summary>
        /// <value>
        /// The weeks.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public long Weeks { get; set; }
        /// <summary>
        /// Gets or sets the payment frequency.
        /// </summary>
        /// <value>
        /// The payment frequency.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string PaymentFrequency { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string StartDate { get; set; } = string.Empty;

        /// <summary>
        /// Sets the family member.
        /// </summary>
        /// <value>
        /// The family member.
        /// </value>        
        public string FamilyMember { get; set; } = string.Empty;
    }
}
