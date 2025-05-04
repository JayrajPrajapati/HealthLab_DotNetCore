namespace HealthLayby.Helpers.Constant
{
    /// <summary>
    /// CustomRegex
    /// </summary>
    public static class CustomRegex
    {
        /// <summary>
        /// The email regex
        /// </summary>
        public const string PhoneRegex = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        /// <summary>
        /// The email regex
        /// </summary>
        public const string EmailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*(?!\.web)\.\w{1,20}([-.]\w{1,20})*$";

        /// <summary>
        /// The password
        /// </summary>
        public const string Password = "^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$";

        /// <summary>
        /// The name
        /// </summary>
        public const string Name = "^[A-Za-z\\s]{1,}[\\.]{0,1}[A-Za-z\\s]{0,}$";

        /// <summary>
        /// The number regex
        /// </summary>
        public const string NumberRegex = "^[0-9]{6,6}$";

        /// <summary>
        /// The month year regex
        /// </summary>
        public const string MonthYearRegex = "^[0-9]{1,2}$";
        /// <summary>
        /// The CVV regex
        /// </summary>
        public const string CVVRegex = "^[0-9]{3,3}$";

        /// <summary>
        /// The card number regex
        /// </summary>
        public const string CardNumberRegex = "^[0-9]{16,16}$";
    }
}
