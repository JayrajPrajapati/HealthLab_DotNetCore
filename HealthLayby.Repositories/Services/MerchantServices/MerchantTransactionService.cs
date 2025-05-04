using HealthLayby.Models.Context;
using HealthLayby.Repositories.Repositories.MerchantRepositories;

namespace HealthLayby.Repositories.Services.MerchantServices
{
    /// <summary>
    /// MerchantTransactionService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.MerchantRepositories.IMerchantTransactionWebRepository" />
    public class MerchantTransactionService:BaseService,IMerchantTransactionWebRepository
    {
        #region Private Variable 
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantTransactionService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MerchantTransactionService(AppDbContext context) : base(context)
        {

        }
        #endregion

        #region Public Methods

        #endregion
    }
}