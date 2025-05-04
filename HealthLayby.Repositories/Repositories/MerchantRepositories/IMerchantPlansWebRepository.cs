using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Models.Models;

namespace HealthLayby.Repositories.Repositories.MerchantRepositories
{
    /// <summary>
    /// IMerchantPlansWebRepository
    /// </summary>
    public interface IMerchantPlansWebRepository
    {
        /// <summary>
        /// Gets the plans list.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<List<MerchantPlansModel>> GetAllPlansList(long merchantId);

        /// <summary>
        /// Gets the active plans list.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<List<MerchantPlansModel>> GetActivePlansList(long merchantId);

        /// <summary>
        /// Gets the completed plans list.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<List<MerchantPlansModel>> GetCompletedPlansList(long merchantId);

        /// <summary>
        /// Gets the pause plans list.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<List<MerchantPlansModel>> GetPausePlansList(long merchantId);

        /// <summary>
        /// Gets the cancel plans list.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<List<MerchantPlansModel>> GetCancelPlansList(long merchantId);

        /// <summary>
        /// Gets the plans details by identifier.
        /// </summary>
        /// <param name="customerPlanId">The customer plan identifier.</param>
        /// <returns></returns>
        Task<MerchantPlansModel> GetPlansDetailsById(long customerPlanId);

        /// <summary>
        /// Determines whether [is merchant exists] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        Task<Merchant> IsMerchantExists(string fullName);

        /// <summary>
        /// Gets the customer details.
        /// </summary>
        /// <param name="customerName">Name of the customer.</param>
        /// <returns></returns>
        Task<Customer> GetCustomerDetails(string customerName);

        /// <summary>
        /// Updates the plan.
        /// </summary>
        /// <param name="customerPlanId">The customer plan identifier.</param>
        /// <param name="submittedOTP">The submitted otp.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<bool> UpdatePlan(long customerPlanId, long submittedOTP,long merchantId);
    }
}
