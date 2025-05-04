using HealthLayby.Helpers.Constant;
using System.ComponentModel.DataAnnotations;

namespace HealthLayby.Models.ApiViewModels.Merchant.Request
{
    public class MerchentListingRequest
    {
        /// <summary>
        /// Gets or sets the sort by.
        /// </summary>
        /// <value>
        /// The sort by.
        /// </value>
        [RegularExpression(CustomRegex.Name, ErrorMessage = MessageConstant.NotValid)]
        public string? sortBy { get; set; }

    }
}
