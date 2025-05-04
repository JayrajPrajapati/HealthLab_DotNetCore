using HealthLayby.Repositories.Repositories;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using HealthLayby.Repositories.Services;
using HealthLayby.Repositories.Services.MerchantServices;
using Stripe;

namespace HealthLayby.Merchant
{
    /// <summary>
    ///   DI Config
    /// </summary>
    public static class DIConfig
    {
        /// <summary>
        /// Registers all services.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAllServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IMerchantWebRepository, MerchantWebService>()
                .AddScoped<IBankRepository, BankService>()
                .AddScoped<IServiceRepository, ServiceService>()
                .AddScoped<IMerchantProfileWebRepository, MerchantProfileService>()
                .AddScoped<IMerchantPasswordWebRepository, MerchantPasswordService>()
                .AddScoped<IMerchantFaqWebRepository, MerchantFaqService>()
                .AddScoped<IMerchantTermsWebRepository, MerchantTermsConditionService>()
                .AddScoped<IMerchantEarningsWebRepository, MerchantEarningsService>()
                .AddScoped<IMerchantTransactionWebRepository, MerchantTransactionService>()
                .AddScoped<IMerchantServicesWebRepository, MerchantServicesService>()
                .AddScoped<IMerchantPlansWebRepository, MerchantPlansService>();
        }

        /// <summary>
        ///   Adds the stripe infrastructure.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static IServiceCollection AddStripeInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            StripeConfiguration.ApiKey = configuration.GetValue<string>("StripeSettings:SecretKey");

            return services
                .AddScoped<Stripe.CustomerService>()
                .AddScoped<ChargeService>()
                .AddScoped<TokenService>()
                .AddScoped<IStripeAppService, StripeAppService>();
        }
    }
}