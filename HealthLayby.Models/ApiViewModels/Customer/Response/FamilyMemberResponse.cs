using System.Text.Json.Serialization;

namespace HealthLayby.Models.ApiViewModels.Customer.Response
{
    /// <summary>
    /// CustomerFamilyMemberForProfile
    /// </summary>
    public class FamilyMemberResponse
    {
        /// <summary>
        /// Gets or sets the family member identifier.
        /// </summary>
        /// <value>
        /// The family member identifier.
        /// </value>
        public long? FamilyMemberId { get; set; } = 0;
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>
        /// The customer identifier.
        /// </value>
        [JsonIgnore]
        public long? CustomerId { get; set; } = 0;
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the relation.
        /// </summary>
        /// <value>
        /// The relation.
        /// </value>
        public string Relation { get; set; } = string.Empty;
    }
}
