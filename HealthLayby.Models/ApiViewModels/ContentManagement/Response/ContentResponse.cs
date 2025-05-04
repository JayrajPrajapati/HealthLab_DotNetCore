using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Models.ApiViewModels.ContentManagement.Response
{
    public class ContentResponse
    {
        /// <summary>
        ///   Gets or sets the page code.
        /// </summary>
        /// <value>
        ///   The page code.
        /// </value>
        public ContentManagementEnum? contentType { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }
    }
}
