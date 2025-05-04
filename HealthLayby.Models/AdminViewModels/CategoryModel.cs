using HealthLayby.Helpers.Constant;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.AdminViewModels
{
    /// <summary>
    /// Category Model
    /// </summary>
    public class CategoryModel
    {
        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public long CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category code.
        /// </summary>
        /// <value>
        /// The category code.
        /// </value>
        public string? CategoryCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        [Display(Name = "Category Name")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [StringLength(LengthConstant.NameMaxLength, ErrorMessage = MessageConstant.NotValid)]
        [Remote("IsCategoryNameAvailable", "Category", AdditionalFields = nameof(CategoryId), HttpMethod = "POST", ErrorMessage = MessageConstant.AlreadyExist)]
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the profile pic.
        /// </summary>
        /// <value>
        /// The profile pic.
        /// </value>
        public string? ProfilePic { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the image base64.
        /// </summary>
        /// <value>
        /// The image base64.
        /// </value>
        public string? ImageBase64 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the image file extension.
        /// </summary>
        /// <value>
        /// The image file extension.
        /// </value>
        public string? ImageFileExtension { get; set; } = string.Empty;

    }
}
