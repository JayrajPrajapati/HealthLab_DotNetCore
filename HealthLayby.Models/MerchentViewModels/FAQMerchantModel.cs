using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.MerchentViewModels
{
    /// <summary>
    ///   FAQMerchantModel
    /// </summary>
    public class FAQMerchantModel
    {
        /// <summary>
        ///   Gets or sets the FAQ identifier.
        /// </summary>
        /// <value>
        ///   The FAQ identifier.
        /// </value>
        public long? FAQId { get; set; }

        /// <summary>
        ///   Gets or sets the question.
        /// </summary>
        /// <value>
        /// The question.
        /// </value>
        [Display(Name = "Question")]
        [Required]
        public string? Question { get; set; }

        /// <summary>
        ///   Gets or sets the answer.
        /// </summary>
        /// <value>
        ///   The answer.
        /// </value>
        [Required]
        [Display(Name = "Answer")]
        public string? Answer { get; set; }
    }
}
