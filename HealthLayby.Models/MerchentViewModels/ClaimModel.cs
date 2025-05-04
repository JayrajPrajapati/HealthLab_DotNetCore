using Microsoft.AspNetCore.Http;

namespace HealthLayby.Models.MerchentViewModels
{
    /// <summary>
    ///   ClaimModel
    /// </summary>
    public class ClaimModel
    {
        /// <summary>
        ///   The HTTP context accessor
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        ///   Initializes a new instance of the <see cref="ClaimModel"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public ClaimModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        ///   Gets the admin identifier.
        /// </summary>
        /// <value>
        /// The admin identifier.
        /// </value>
        public long AdminId
        {
            get
            {
                if (long.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst("MerchantId")?.Value, out var adminId))
                    return adminId;
                
                return 0;
            }
        }

        /// <summary>
        ///   Gets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.FindFirst("FirstName")?.Value.ToString() ?? string.Empty;
            }
        }

        /// <summary>
        ///   Gets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.FindFirst("LastName")?.Value.ToString() ?? string.Empty;
            }
        }

        /// <summary>
        ///   Gets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public string EmailAddress
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.FindFirst("EmailAddress")?.Value.ToString() ?? string.Empty;
            }
        }
    }
}
