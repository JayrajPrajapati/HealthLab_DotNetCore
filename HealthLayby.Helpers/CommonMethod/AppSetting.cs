namespace HealthLayby.Helpers.CommonMethod
{
    /// <summary>
    /// AppSetting
    /// </summary>
    public class AppSetting
    {
        /// <summary>
        /// Gets or sets the client email.
        /// </summary>
        /// <value>
        /// The client email.
        /// </value>
        public static string ClientEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the client email password.
        /// </summary>
        /// <value>
        /// The client email password.
        /// </value>
        public static string ClientEmailPassword { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is SSL enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is SSL enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool IsSSLEnabled { get; set; } = false;
    }
}
