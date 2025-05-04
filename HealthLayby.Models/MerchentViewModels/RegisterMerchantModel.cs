using HealthLayby.Helpers.Constant;
using HealthLayby.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthLayby.Models.MerchentViewModels
{
    /// <summary>
    /// RegisterMerchantModel
    /// </summary>
    public class RegisterMerchantModel
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Display(Name = "First Name")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.Name, ErrorMessage = MessageConstant.Required)]        
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Display(Name = "Last Name")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.Name, ErrorMessage = MessageConstant.Required)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Display(Name = "Email")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.EmailRegex, ErrorMessage = MessageConstant.NotValid)]
        [MaxLength(LengthConstant.EmailMaxLength, ErrorMessage = MessageConstant.EmailAddressMaxLength)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the phone number.
        /// </summary>
        /// <value>
        ///   The phone.
        /// </value>
        [Display(Name = "Phone")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [DataType(DataType.PhoneNumber)]
        [StringLength(LengthConstant.PhoneNumberMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [Display(Name = "Category")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public long Category { get; set; }

        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>
        [Display(Name = "Service")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public string[] ServiceId { get; set; } 

        /// <summary>
        /// Gets or sets the name of the clinic.
        /// </summary>
        /// <value>
        /// The name of the clinic.
        /// </value>
        [Display(Name = "Clinic Name")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.Name, ErrorMessage = MessageConstant.Required)]
        public string ClinicName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        [Display(Name = "Notes")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public string Notes { get; set; } = string.Empty;
    }
}
