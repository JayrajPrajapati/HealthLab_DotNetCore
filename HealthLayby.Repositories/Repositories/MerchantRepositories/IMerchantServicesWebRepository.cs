using HealthLayby.Models.MerchentViewModels;

namespace HealthLayby.Repositories.Repositories.MerchantRepositories
{
    /// <summary>
    /// IMerchantServicesWebRepository
    /// </summary>
    public interface IMerchantServicesWebRepository
    {

        /// <summary>
        /// Gets the service details by login.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<List<MerchantServiceModel>> GetServiceDetailsByLogin(long merchantId);

        /// <summary>
        /// Gets the service details by serivce identifier.
        /// </summary>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns></returns>
        Task<MerchantServiceModel> GetServiceDetailsBySerivceId(long serviceId);

        /// <summary>
        /// Updates the service.
        /// </summary>
        /// <param name="merchantServiceModel">The merchant service model.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<bool> UpdateService(MerchantServiceModel merchantServiceModel,long merchantId);

        /// <summary>
        /// Updates the service status.
        /// </summary>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<bool> UpdateServiceStatus(long serviceId, long merchantId);
    }
}
