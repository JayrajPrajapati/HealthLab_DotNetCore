using HealthLayby.Helpers.Constant;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.ApiViewModels.Customer.Request
{
    /// <summary>
    /// UpdateProfileDetails
    /// </summary>
    public class UpdateProfileDetailsRequest
    {
        
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [StringLength(LengthConstant.NameMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [StringLength(LengthConstant.NameMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.EmailRegex, ErrorMessage = MessageConstant.NotValid)]
        [StringLength(LengthConstant.EmailMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [DataType(DataType.PhoneNumber)]
        [StringLength(LengthConstant.PhoneNumberMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the profile pic.
        /// </summary>
        /// <value>
        /// The profile pic.
        /// </value>
        public string? ProfilePic { get; set; }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the customer address.
        /// </summary>
        /// <value>
        /// The customer address.
        /// </value>
        public string? CustomerAddress { get; set; }

        /// <summary>
        /// Gets or sets the profile image.
        /// </summary>
        /// <value>
        /// The profile image.
        /// </value>
        public IFormFile? ProfileImage { get; set; }
    }
}
