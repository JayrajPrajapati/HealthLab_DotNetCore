using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Helpers.Attributes
{
    /// <summary>
    /// FileMaxSizeAttribute
    /// </summary>
    /// <seealso cref="System.ComponentModel.DataAnnotations.ValidationAttribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FileMaxSizeAttribute : ValidationAttribute 
    {
        /// <summary>
        /// The maximum file size
        /// </summary>
        private readonly int _maxFileSize;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileMaxSizeAttribute"/> class.
        /// </summary>
        public FileMaxSizeAttribute()
        {                        
            _maxFileSize = 2 * 1024 * 1024;            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileMaxSizeAttribute"/> class.
        /// </summary>
        /// <param name="maxFileSize">Maximum size of the file.</param>
        public FileMaxSizeAttribute(int? maxFileSize = null)
        {
            if (maxFileSize.HasValue)
            {
                _maxFileSize = maxFileSize.Value;
            }
            else
            {
                _maxFileSize = 2 * 1024 * 1024;
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

            if (file.Length > _maxFileSize)
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
            return $"Maximum allowed file size is {_maxFileSize} bytes.";
        }
    }
}
