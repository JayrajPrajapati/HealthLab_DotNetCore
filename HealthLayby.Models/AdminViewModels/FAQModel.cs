using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.AdminViewModels
{
    /// <summary>
    /// FAQ Model
    /// </summary>
    public class FAQModel
    {
        /// <summary>
        /// Gets or sets the FAQ identifier.
        /// </summary>
        /// <value>
        /// The FAQ identifier.
        /// </value>
        public long? FAQId { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>
        /// The question.
        /// </value>
        [Display(Name = "Question")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public string? Question { get; set; }

        /// <summary>
        /// Gets or sets the answer.
        /// </summary>
        /// <value>
        /// The answer.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [Display(Name = "Answer")]
        public string? Answer { get; set; }
    }
}
