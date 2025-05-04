namespace HealthLayby.API.Infrastructure
{
    public class ClaimModel
    {

        #region Private Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructor
        public ClaimModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }
        #endregion


        public long CustomerId
        {
            get
            {
                if (long.TryParse(_httpContextAccessor.HttpContext?.User.FindFirst("CustomerId")?.Value, out var CustomerId))
                    return CustomerId;

                return 0;
            }
        }
    }
}
