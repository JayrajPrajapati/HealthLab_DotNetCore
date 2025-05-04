using HealthLayby.Models.ApiViewModels.CustomerPlans.Request;
using HealthLayby.Models.ApiViewModels.CustomerPlans.Response;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// ICustomerPlansRepository
    /// </summary>
    public interface ICustomerPlansRepository
    {
        /// <summary>
        /// Gets the plan list.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        Task<(bool, string, List<CustomerPlansResponse>)> GetPlanList(long customerId);
        /// <summary>
        /// Gets the plan review details.
        /// </summary>
        /// <param name="CustomerId">The customer identifier.</param>
        /// <param name="planReviewRequestModel">The plan review request model.</param>
        /// <returns></returns>
        Task<(bool, string, PlanReviewResponse)> GetPlanViewDetails(long CustomerId,PlanViewRequestModel planReviewRequestModel);

        /// <summary>
        /// Gets the agreement.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="agreementRequest">The agreement request.</param>
        /// <returns></returns>
        Task<(bool, string, AgreementResponse)> GetAgreement(long customerId, AgreementRequest agreementRequest);
    }
}
