using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Models.MerchentViewModels
{
    /// <summary>
    /// TermsConditionModel
    /// </summary>
    public class TermsConditionModel
    {        
        /// <summary>
        ///   Gets or sets the description.
        /// </summary>
        /// <value>
        ///   The description.
        /// </value>
        public string? Description { get; set; }

        /// <summary>
        ///   Gets or sets the page code.
        /// </summary>
        /// <value>
        ///   The page code.
        /// </value>
        public ContentManagementEnum PageCode { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string Title { get; set; } = string.Empty;

    }
}
