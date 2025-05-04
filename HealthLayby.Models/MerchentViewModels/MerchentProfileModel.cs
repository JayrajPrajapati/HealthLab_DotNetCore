using HealthLayby.Helpers.Constant;
using HealthLayby.Models.Models;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.MerchentViewModels
{
    /// <summary>
    /// MerchentProfileModel
    /// </summary>
    public class MerchentProfileModel
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string firstName { get;set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string lastName { get;set; }
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string emailAddress { get; set; }
        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string phoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the clinics.
        /// </summary>
        /// <value>
        /// The clinics.
        /// </value>
        //public List<Clinic> clinics { get; set; }

        /// <summary>
        /// Gets or sets the name of the clinic.
        /// </summary>
        /// <value>
        /// The name of the clinic.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string clinicName { get; set; }

        /// <summary>
        /// Gets or sets the clinic location.
        /// </summary>
        /// <value>
        /// The clinic location.
        /// </value>
        public string clinicLocation { get; set; }

        /// <summary>
        /// Gets or sets the name of the bank.
        /// </summary>
        /// <value>
        /// The name of the bank.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string bankName { get; set; }
        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        /// <value>
        /// The account number.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string accountNumber { get; set;}
        /// <summary>
        /// Gets or sets the BSB number.
        /// </summary>
        /// <value>
        /// The BSB number.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string bsbNumber { get; set; }

        /// <summary>
        /// Gets or sets the bank location.
        /// </summary>
        /// <value>
        /// The bank location.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string bankLocation { get; set; }

        /// <summary>
        /// Gets or sets the profile image.
        /// </summary>
        /// <value>
        /// The profile image.
        /// </value>
        public string? profileImage { get; set; }

        /// <summary>
        /// Gets or sets the logo.
        /// </summary>
        /// <value>
        /// The logo.
        /// </value>
        public string? Logo { get; set; }

        /// <summary>
        ///   Gets or sets the image base64.
        /// </summary>
        /// <value> The image base64. </value>
        public string? ImageBase64 { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the image file extension.
        /// </summary>
        /// <value>The image file extension.</value>
        public string? ImageFileExtension { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the logo base64.
        /// </summary>
        /// <value>The logo base64. </value>
        public string? LogoBase64 { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the logo file extension.
        /// </summary>
        /// <value> The logo file extension.</value>
        public string? LogoFileExtension { get; set; } = string.Empty;

    }
}