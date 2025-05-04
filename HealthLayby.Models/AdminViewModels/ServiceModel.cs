using HealthLayby.Helpers.Constant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.AdminViewModels
{
    /// <summary>
    /// ServiceModel
    /// </summary>
    public class ServiceModel
    {
        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>
        public long ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the categories.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        [Required(ErrorMessage = "Please select category!")]
        public long CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        [Display(Name = "Service Name")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [StringLength(50, ErrorMessage = MessageConstant.NotValid)]
        [Remote("IsServiceNameAvailable", "Service", AdditionalFields = $"{nameof(ServiceId)},{nameof(CategoryId)}", HttpMethod = "POST", ErrorMessage = MessageConstant.AlreadyExist)]
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Display(Name = "Description")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [MaxLength(2000, ErrorMessage = "Name cannot be longer than 2000 characters.")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the service thumbnail.
        /// </summary>
        /// <value>
        /// The service thumbnail.
        /// </value>
        public IFormFile? ServiceThumbnail { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        [Display(Name = "Price")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [Range(0.01, int.MaxValue, ErrorMessage = MessageConstant.PriceValidation)]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName { get; set; } = string.Empty;

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
