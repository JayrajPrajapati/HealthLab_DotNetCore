namespace HealthLayby.Models.ApiViewModels.Category
{
    /// <summary>
    /// CategoryListResponse
    /// </summary>
    public class CategoryListResponse
    {
        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public long CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the category code.
        /// </summary>
        /// <value>
        /// The category code.
        /// </value>
        public string CategoryCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public string Image { get; set; } = string.Empty;

    }
}
