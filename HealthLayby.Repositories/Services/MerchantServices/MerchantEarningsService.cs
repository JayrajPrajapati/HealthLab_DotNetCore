using HealthLayby.Models.Context;
using HealthLayby.Repositories.Repositories.MerchantRepositories;

namespace HealthLayby.Repositories.Services.MerchantServices
{
    /// <summary>
    /// MerchantEarningsService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.MerchantRepositories.IMerchantEarningsWebRepository" />
    public class MerchantEarningsService : BaseService,IMerchantEarningsWebRepository
    {
        #region Private Variable 
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantTermsConditionService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MerchantEarningsService(AppDbContext context) : base(context)
        {

        }
        #endregion

        #region Public Methods

        #endregion
    }
}
