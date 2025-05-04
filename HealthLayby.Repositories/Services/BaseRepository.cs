using HealthLayby.Models.Context;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// base service
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// The context
        /// </summary>
        protected AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public BaseService(AppDbContext context)
        {
            _context = context;
        }
    }
}
