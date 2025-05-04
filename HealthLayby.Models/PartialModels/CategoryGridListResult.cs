namespace HealthLayby.Models.Models
{
    /// <summary>
    ///   CategoryGridListResult
    /// </summary>
    public partial class CategoryGridListResult
    {

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public long CategoryId { get; set; }

        /// <summary>
        ///   Gets or sets the category code.
        /// </summary>
        /// <value>
        /// The category code.
        /// </value>
        public string? CategoryCode { get; set; }

        /// <summary>
        ///   Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string? CategoryName { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is active; otherwise, <c>false</c>.
        /// </value>
        public bool IsActive { get; set; }
    }
}
