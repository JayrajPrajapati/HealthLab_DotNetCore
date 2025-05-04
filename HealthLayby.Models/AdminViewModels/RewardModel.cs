using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.AdminViewModels
{
    /// <summary>
    /// Reward Model
    /// </summary>
    public class RewardModel
    {
        /// <summary>
        /// Gets or sets the reward identifier.
        /// </summary>
        /// <value>
        /// The reward identifier.
        /// </value>
        public long RewardId { get; set; }

        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        [Display(Name = "Merchant")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public long MerchantId { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        [Display(Name = "Category")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public long CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        [Display(Name = "Month")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Status")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the type of the discount.
        /// </summary>
        /// <value>
        /// The type of the discount.
        /// </value>
        [Display(Name = "Discount Type")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public int DiscountType { get; set; }

        /// <summary>
        /// Gets or sets the discount.
        /// </summary>
        /// <value>
        /// The discount.
        /// </value>
        [Display(Name = "Discount")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public decimal Discount { get; set; }
    }
}
