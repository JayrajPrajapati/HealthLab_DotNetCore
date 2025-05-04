using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.ApiViewModels.Customer.Request
{
    /// <summary>
    /// SaveFamilyMemberRequest
    /// </summary>
    public class SaveFamilyMemberRequest
    {
        /// <summary>
        /// Gets or sets the family member identifier.
        /// </summary>
        /// <value>
        /// The family member identifier.
        /// </value>
        public long? FamilyMemberId { get; set; }       

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
        [StringLength(LengthConstant.NameMaxLength, ErrorMessage = MessageConstant.NotValid)]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the relation.
        /// </summary>
        /// <value>
        /// The relation.
        /// </value>
        [Required(ErrorMessage = MessageConstant.Required)]
        public string Relation { get; set; } = string.Empty;
    }
}
