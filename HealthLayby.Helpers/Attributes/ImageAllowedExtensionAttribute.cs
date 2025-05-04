using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Helpers.Attributes
{
    /// <summary>
    /// ImageAllowedExtensionAttribute
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ImageAllowedExtensionAttribute : ValidationAttribute
    {
        /// <summary>
        /// The extensions
        /// </summary>
        private readonly string[] _extensions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageAllowedExtensionAttribute"/> class.
        /// </summary>
        /// <param name="extensions">The extensions.</param>
        public ImageAllowedExtensionAttribute(string[]? extensions = null)
        {
            if (extensions == null)
            {
                _extensions = new string[] { ".png", ".jpg", ".jpeg" };
            }
            else
            {
                _extensions = extensions;
            }            
        }

        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult" /> class.
        /// </returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            var file = value as IFormFile;

            if (file == null)
            {
                return ValidationResult.Success;
            }

            var extension = Path.GetExtension(file.FileName);

            if (string.IsNullOrWhiteSpace(extension))
            {
                return new ValidationResult(GetErrorMessage());
            }

            if (!_extensions.Contains(extension.ToLower()))
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage()
        {
            return $"This photo extension is not allowed!";
        }
    }
}
