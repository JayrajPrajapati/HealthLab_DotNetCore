using HealthLayby.Helpers.Constant;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.AdminViewModels
{
    /// <summary>
    /// merchantRequestModel
    /// </summary>
    public class MerchantRequestModel
    {
        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public long MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the reject reason.
        /// </summary>
        /// <value>
        /// The reject reason.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [Display(Name = "Reject Reason")]
        [MaxLength(2000, ErrorMessage = "Name cannot be longer than 2000 characters.")]
        public string RejectReason { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category list.
        /// </summary>
        /// <value>
        /// The category list.
        /// </value>
        public List<SelectListItem> CategoryList { get; set; } = null!;
        /// <summary>
        /// Gets or sets the service list.
        /// </summary>
        /// <value>
        /// The service list.
        /// </value>
        public List<SelectListItem> ServiceList { get; set; } = null!;

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public long CategoryId { get; set; }
        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>    
        [Required(ErrorMessage = MessageConstant.Required)]
        public string ServiceId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the service ids.
        /// </summary>
        /// <value>
        /// The service ids.
        /// </value>
        public string ServiceIds { get; set; } = string.Empty;
    }
}
