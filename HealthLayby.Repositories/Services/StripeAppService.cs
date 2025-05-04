using HealthLayby.Models.StripeModels;
using HealthLayby.Repositories.Repositories;
using Stripe;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// Stripe service
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Repositories.IStripeAppService" />
    public class StripeAppService : IStripeAppService
    {
        /// <summary>
        /// The charge service
        /// </summary>
        private readonly ChargeService _chargeService;

        /// <summary>
        /// The customer service
        /// </summary>
        private readonly Stripe.CustomerService _customerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="StripeAppService" /> class.
        /// </summary>
        /// <param name="chargeService">The charge service.</param>
        /// <param name="customerService">The customer service.</param>
        public StripeAppService(ChargeService chargeService,
                                Stripe.CustomerService customerService)
        {
            _chargeService = chargeService;
            _customerService = customerService;
        }

        /// <summary>
        /// Create a new customer at Stripe through API using customer and card details from records.
        /// </summary>
        /// <param name="customer">Stripe Customer</param>
        /// <returns>
        /// Stripe Customer
        /// </returns>
        public async Task<StripeCustomer> AddStripeCustomerAsync(CustomerCreateOptions customer)
        {
            // Create customer at Stripe
            var createdCustomer = await _customerService.CreateAsync(customer);

            // Return the created customer at stripe
            return new StripeCustomer
            {
                CustomerId = createdCustomer.Id,
                Email = createdCustomer.Email,
                Name = createdCustomer.Name
            };
        }

        /// <summary>
        /// Add Stripe Paymnet
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <returns></returns>
        public async Task<StripePayment> AddStripePaymentAsync(AddStripePayment payment)
        {
            // Set the options for the payment we would like to create at Stripe
            ChargeCreateOptions paymentOptions = new ChargeCreateOptions
            {
                Customer = payment.CustomerId,
                ReceiptEmail = payment.ReceiptEmail,
                Description = payment.Description,
                Currency = payment.Currency,
                Amount = payment.Amount
            };

            // Create the payment
            var createdPayment = await _chargeService.CreateAsync(paymentOptions);

            // Return the payment to requesting method
            return new StripePayment()
            {
                Amount = createdPayment.Amount,
                Currency = createdPayment.Currency,
                CustomerId = createdPayment.CustomerId,
                Description = createdPayment.Description,
                ReceiptEmail = createdPayment.ReceiptEmail,
                PaymentId = createdPayment.Id
            };
        }

        /// <summary>
        /// Updates the stripe customer asynchronous.
        /// </summary>
        /// <param name="stripeCustomerId">The stripe customer identifier.</param>
        /// <param name="customer">The customer.</param>
        /// <returns></returns>
        public async Task<StripeCustomer> UpdateStripeCustomerAsync(string stripeCustomerId, CustomerUpdateOptions customer)
        {
            var updatedCustomer = await _customerService.UpdateAsync(stripeCustomerId, customer);

            // Return the created customer at stripe
            return new StripeCustomer
            {
                CustomerId = updatedCustomer.Id,
                Email = updatedCustomer.Email,
                Name = updatedCustomer.Name
            };
        }
    }
}
