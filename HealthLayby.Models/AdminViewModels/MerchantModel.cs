using HealthLayby.Helpers.Constant;
using HealthLayby.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthLayby.Models.AdminViewModels
{
    /// <summary>
    /// Merchant Model
    /// </summary>
    public class MerchantModel
    {
        /// <summary>
        /// Gets or sets the merchant identifier.
        /// </summary>
        /// <value>
        /// The merchant identifier.
        /// </value>
        public long MerchantId { get; set; }

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
        [Display(Name = "Email")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.EmailRegex, ErrorMessage = MessageConstant.NotValid)]
        [StringLength(LengthConstant.EmailMaxLength, ErrorMessage = MessageConstant.NotValid)]
        [Remote("IsMerchantEmailExist", "Merchant", AdditionalFields = nameof(MerchantId), HttpMethod = "POST", ErrorMessage = "Email is alreay exists!")]
        public string EmailAddress { get; set; } = string.Empty;


        /// <summary>
        /// Gets or sets the clinic identifier.
        /// </summary>
        /// <value>
        /// The clinic identifier.
        /// </value>
        public long ClinicId { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        [Required(ErrorMessage = MessageConstant.SelectCategory)]
        public long CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>
        [Required(ErrorMessage = MessageConstant.SelectService)]
        public string ServiceId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        /// [Display(Name = "Password")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [RegularExpression(CustomRegex.Password, ErrorMessage = MessageConstant.PasswordNotValid)]
        [StringLength(maximumLength: 16, MinimumLength = 6, ErrorMessage = MessageConstant.PasswordMinMaxLength)]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Status")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public bool IsActive { get; set; }

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
        /// Gets or sets the profile pic.
        /// </summary>
        /// <value>
        /// The profile pic.
        /// </value>
        public string? ProfilePic { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>
        /// The logo.
        /// </value>
        public string? Logo { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>
        /// The full name.
        /// </value>
        public string? FullName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the created on.
        /// </summary>
        /// <value>
        /// The created on.
        /// </value>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the name of the bank.
        /// </summary>
        /// <value>
        /// The name of the bank.
        /// </value>
        [Display(Name = "Bank")]
        [StringLength(LengthConstant.HundredLength, ErrorMessage = MessageConstant.NotValid)]
        public string BankName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the branch.
        /// </summary>
        /// <value>
        /// The name of the branch.
        /// </value>
        [Display(Name = "Bank Location (Branch)")]
        [StringLength(LengthConstant.FiveHundredLength, ErrorMessage = MessageConstant.NotValid)]
        public string BranchName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the bank account no.
        /// </summary>
        /// <value>
        /// The bank account no.
        /// </value>
        [Display(Name = "Bank Account Number")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public string BankAccountNo { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the BSB no.
        /// </summary>
        /// <value>
        /// The BSB no.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        [Display(Name = "BSB Number")]
        public string BSBNo { get; set; } = string.Empty;


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

        /// <summary>
        /// Gets or sets the logo base64.
        /// </summary>
        /// <value>
        /// The logo base64.
        /// </value>
        public string? LogoBase64 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the logo file extension.
        /// </summary>
        /// <value>
        /// The logo file extension.
        /// </value>
        public string? LogoFileExtension { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public string Services { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the clinic.
        /// </summary>
        /// <value>
        /// The name of the clinic.
        /// </value>
        [Display(Name = "Clinic or Lab Name")]
        [Required(ErrorMessage = MessageConstant.Required)]
        [StringLength(LengthConstant.ThrityLength, ErrorMessage = MessageConstant.NotValid)]
        public string ClinicName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [Display(Name = "Location")]
        [Required(ErrorMessage = MessageConstant.Required)]
        public string ClinicLocation { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the category list.
        /// </summary>
        /// <value>
        /// The category list.
        /// </value>
        [ValidateNever]
        public List<SelectListItem> CategoryList { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service list.
        /// </summary>
        /// <value>
        /// The service list.
        /// </value>
        [ValidateNever]
        public List<SelectListItem> ServiceList { get; set; } = null!;

        /// <summary>
        /// Gets or sets the service identifier.
        /// </summary>
        /// <value>
        /// The service identifier.
        /// </value>
        [ValidateNever]
        public string ServiceIds { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        [ValidateNever]
        public decimal Amount { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is rejected.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is rejected; otherwise, <c>false</c>.
        /// </value>
        public bool IsRejected { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        [ValidateNever]
        public string CategoryName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the services names.
        /// </summary>
        /// <value>
        /// The services names.
        /// </value>
        [ValidateNever]
        public string ServicesNames { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the accepted.
        /// </summary>
        /// <value>
        /// The accepted.
        /// </value>
        [ValidateNever]
        public DateTime? Accepted { get; set; } 

    }
}
