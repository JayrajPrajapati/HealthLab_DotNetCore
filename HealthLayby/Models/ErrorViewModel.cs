namespace HealthLayby.Admin.Models
{
    /// <summary>
    ///   Error View Model
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        ///   Gets or sets the request identifier.
        /// </summary>
        /// <value>
        ///   The request identifier.
        /// </value>
        public string? RequestId { get; set; }

        /// <summary>
        ///   Gets a value indicating whether [show request identifier].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show request identifier]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}