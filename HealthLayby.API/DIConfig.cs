using HealthLayby.API.Infrastructure.UploadContent;
using HealthLayby.Repositories.Repositories;
using HealthLayby.Repositories.Services;
using Stripe;

namespace HealthLayby.API
{
    /// <summary>
    /// DIConfig
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
                .AddScoped<ICustomerForgotPasswordTokenRepository, CustomerForgotPasswordTokenService>()
                .AddScoped<ICustomerRepository, HealthLayby.Repositories.Services.CustomerService>()
                .AddScoped<IMerchantRepository, MerchantService>()
                .AddScoped<IMerchantRequestRepository, MerchantRequestService>()
                .AddScoped<ICategoryRepository, CategoryService>()
                .AddScoped<IServiceRepository, ServiceService>()
                .AddScoped<ICMSRepository, CMSService>()
                .AddScoped<ITransactionHistoryRepository, TransactionHistoryService>()
                .AddScoped<IRewardsRepository, RewardsService>()
                .AddScoped<IGeneralRepository, GeneralService>()
                .AddScoped<IUploadContentHelper, UploadContentHelper>()
                .AddScoped<IBankRepository, BankService>()
                .AddScoped<ICreditCardRepository, CreditCardService>()
                .AddScoped<ICustomerPlansRepository,CustomerPlansService>()
                .AddScoped<IHomeRepository, HomeService>()
                .AddScoped<ITransactionRepository, TransactionService>();
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
