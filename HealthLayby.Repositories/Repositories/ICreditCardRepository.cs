using HealthLayby.Models.ApiViewModels.CreditCard.Request;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// ICreditCardRepository
    /// </summary>
    public interface ICreditCardRepository
    {
        /// <summary>
        /// Saves the bank details.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="saveUpdateCreditCardRequest">The save update credit card request.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveUpdateCreditCardDetails(long customerId, SaveUpdateCreditCardRequest saveUpdateCreditCardRequest);
    }
}
