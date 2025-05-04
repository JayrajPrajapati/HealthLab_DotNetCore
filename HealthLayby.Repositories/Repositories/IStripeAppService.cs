using HealthLayby.Models.StripeModels;
using Stripe;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// IStripeAppService
    /// </summary>
    public interface IStripeAppService
    {
        /// <summary>
        /// Adds the stripe customer asynchronous.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns></returns>
        Task<StripeCustomer> AddStripeCustomerAsync(CustomerCreateOptions customer);

        /// <summary>
        /// Adds the stripe payment asynchronous.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <returns></returns>
        Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment);

        /// <summary>
        /// Updates the stripe customer asynchronous.
        /// </summary>
        /// <param name="stripeCustomerId">The stripe customer identifier.</param>
        /// <param name="customer">The customer.</param>
        /// <returns></returns>
        Task<StripeCustomer> UpdateStripeCustomerAsync(string stripeCustomerId, CustomerUpdateOptions customer);
    }
}
