using HealthLayby.Models.ApiViewModels.Transaction.Response;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// Transaction Repository
    /// </summary>
    public interface ITransactionRepository
    {
        /// <summary>
        /// Gets the transaction list.
        /// </summary>
        /// <returns></returns>
        Task<(bool, string,TransactionResponse)> GetTransactionList(); 
    }
}
