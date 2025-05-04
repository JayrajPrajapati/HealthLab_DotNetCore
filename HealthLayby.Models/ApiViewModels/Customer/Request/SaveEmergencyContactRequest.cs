using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.ApiViewModels.Customer.Request
{
    /// <summary>
    /// SaveEmergencyContactRequest
    /// </summary>
    public class SaveEmergencyContactRequest
    {

        /// <summary>
        /// Gets or sets the emergency contact identifier.
        /// </summary>
        /// <value>
        /// The emergency contact identifier.
        /// </value>
        public long? EmergencyContactId { get; set; }

        /// <summary>
        /// Gets or sets the first name of the emergency.
        /// </summary>
        /// <value>
        /// The first name of the emergency.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [StringLength(LengthConstant.NameMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the emergency.
        /// </summary>
        /// <value>
        /// The last name of the emergency.
        /// </value>
        [StringLength(LengthConstant.NameMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string? LastName { get; set; }

        /// <summary>
        /// Gets or sets the emergency mobile number.
        /// </summary>
        /// <value>
        /// The emergency mobile number.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [DataType(DataType.PhoneNumber)]
        [StringLength(LengthConstant.PhoneNumberMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string Number { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the emergency contact email.
        /// </summary>
        /// <value>
        /// The emergency contact email.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.EmailRegex, ErrorMessage = MessageConstant.NotValid)]
        [StringLength(LengthConstant.EmailMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string Email { get; set; } = string.Empty;
    }
}
