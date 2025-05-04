using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.AdminViewModels
{
    /// <summary>
    /// Update Admin Profile Model
    /// </summary>
    public class UpdateAdminProfileModel
    {
        /// <summary>
        /// Gets or sets the admin identifier.
        /// </summary>
        /// <value>
        /// The admin identifier.
        /// </value>
        public long AdminId { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Display(Name = "First Name")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [StringLength(LengthConstant.NameMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [StringLength(LengthConstant.NameMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.EmailRegex, ErrorMessage = MessageConstant.NotValid)]
        public string EmailAddress { get; set; } = string.Empty;
    }
}
