using HealthLayby.Helpers.Constant;
using HealthLayby.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.AdminViewModels
{
    /// <summary>
    /// customer model
    /// </summary>
    public class CustomerModel
    {
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        public long CustomerId { get; set; }

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
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [Display(Name = "Email")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.EmailRegex, ErrorMessage = MessageConstant.NotValid)]
        [StringLength(LengthConstant.EmailMaxLength, ErrorMessage = MessageConstant.NotValid)]
        [Remote("IsCustomerEmailAvailable", "Customer", AdditionalFields = nameof(CustomerId), HttpMethod = "POST", ErrorMessage = MessageConstant.AlreadyExist)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        [Display(Name = "Phone")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [DataType(DataType.PhoneNumber)]
        [StringLength(LengthConstant.PhoneNumberMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the first name of the emergency.
        /// </summary>
        /// <value>
        /// The first name of the emergency.
        /// </value>
        [Display(Name = "First Name")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [StringLength(LengthConstant.NameMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string EmergencyFirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the emergency.
        /// </summary>
        /// <value>
        /// The last name of the emergency.
        /// </value>
        [Display(Name = "Last Name")]
        [StringLength(LengthConstant.NameMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string? EmergencyLastName { get; set; }

        /// <summary>
        /// Gets or sets the emergency mobile number.
        /// </summary>
        /// <value>
        /// The emergency mobile number.
        /// </value>
        [Display(Name = "Emergency Number")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [DataType(DataType.PhoneNumber)]
        [StringLength(LengthConstant.PhoneNumberMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string EmergencyMobileNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the registered on.
        /// </summary>
        /// <value>
        /// The registered on.
        /// </value>
        public string RegisteredOn { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the plans.
        /// </summary>
        /// <value>
        /// The plans.
        /// </value>
        public int Plans { get; set; }

        /// <summary>
        /// Gets or sets the wallet amt.
        /// </summary>
        /// <value>
        /// The wallet amt.
        /// </value>
        public decimal WalletAmt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets the emergency email.
        /// </summary>
        /// <value>
        /// The emergency email.
        /// </value>
        [Display(Name = "Emergency Email")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.EmailRegex, ErrorMessage = MessageConstant.NotValid)]
        [StringLength(LengthConstant.EmailMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string EmergencyEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the first name of the family member.
        /// </summary>
        /// <value>
        /// The first name of the family member.
        /// </value>
        [ValidateNever]
        public string FamilyMemberFirstName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name of the family member.
        /// </summary>
        /// <value>
        /// The last name of the family member.
        /// </value>
        [ValidateNever]
        public string FamilyMemberLastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the family relation.
        /// </summary>
        /// <value>
        /// The family relation.
        /// </value>
        [ValidateNever]
        public string FamilyRelation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the customer emergency contacts.
        /// </summary>
        /// <value>
        /// The customer emergency contacts.
        /// </value>
        [ValidateNever]
        public List<CustomerEmergencyContact> customerEmergencyContacts { get; set; } = null!;

        /// <summary>
        /// Gets or sets the profile pic.
        /// </summary>
        /// <value>
        /// The profile pic.
        /// </value>
        [ValidateNever]
        public string ProfilePic { get; set; }
    }
}
