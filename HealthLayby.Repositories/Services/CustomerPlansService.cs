using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels.CustomerPlans.Request;
using HealthLayby.Models.ApiViewModels.CustomerPlans.Response;
using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// CustomerPlansService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.ICustomerPlansRepository" />
    public class CustomerPlansService : BaseService, ICustomerPlansRepository
    {
        #region Private Variable 

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCardService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CustomerPlansService(AppDbContext context) : base(context)
        {
            //  _stripeAppService = stripeAppService;
        }

        #endregion

        #region Mobile API's

        /// <summary>
        /// Gets the plan list.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string, List<CustomerPlansResponse>)> GetPlanList(long customerId)
        {
            List<CustomerPlansResponse> customerPlansResponsesList = new List<CustomerPlansResponse>(); List<CustomerPlans> customerPlanList = new List<CustomerPlans>();
            CustomerPlansResponse customerPlans = new CustomerPlansResponse(); Merchant merchant = new Merchant();
            Service service = new Service(); Category category = new Category();

            var message = string.Empty;
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                customerPlanList = await _context.CustomerPlans.Where(x => x.CustomerId == customerId && x.IsActive == true && x.IsDelete == false).ToListAsync();
                if (customerPlanList.Count > 0)
                {
                    foreach (var list in customerPlanList)
                    {
                        customerPlans = new CustomerPlansResponse();
                        merchant = _context.Merchant.Where(y => y.MerchantId == list.MerchantId && y.IsDeleted == false && y.IsActive == true).FirstOrDefault();
                        service = _context.Service.Where(z => z.ServiceId == list.MasterServiceId && z.IsDeleted == false && z.IsActive == true).FirstOrDefault();
                        if (service != null)
                            category = _context.Category.Where(xy => xy.CategoryId == service.CategoryId).FirstOrDefault();

                        customerPlans.CustomerPlanId = list.CustomerPlanId;
                        customerPlans.CustomerId = list.CustomerId;
                        customerPlans.MerchantName = merchant == null ? "" : merchant.FullName;
                        customerPlans.MasterServiceName = service == null ? "" : service.ServiceName;
                        customerPlans.CategoryName = category == null ? "" : category.CategoryName;
                        customerPlans.PlanImage = list.PlanImage;
                        customerPlans.PlanName = list.PlanName;
                        customerPlans.IsActive = list.IsActive;
                        customerPlans.StartDate = list.StartDate;
                        customerPlans.EndDate = list.EndDate;

                        customerPlansResponsesList.Add(customerPlans);
                    }
                    message = ApiMessages.SuccessfullPlanList;
                }
                else
                    message = ApiMessages.NoPlanList;

                if (customerPlansResponsesList.Count > 0)
                    return (true, message, customerPlansResponsesList);
                else
                    return (false, ApiMessages.InvalidPlanList, null);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Gets the plan review details.
        /// </summary>
        /// <param name="CustomerId">The customer identifier.</param>
        /// <param name="planReviewRequestModel">The plan review request model.</param>
        /// <returns></returns>
        public async Task<(bool, string, PlanReviewResponse)> GetPlanViewDetails(long CustomerId, PlanViewRequestModel planReviewRequestModel)
        {
            CustomerPlans customerPlan = new CustomerPlans();
            ServiceDetails serviceDetails = new ServiceDetails();
            Category category = new Category();
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                customerPlan = await _context.CustomerPlans.Where(x => x.CustomerPlanId == planReviewRequestModel.CustomerPlanId && x.CustomerId == CustomerId && x.IsActive == true && !x.IsDelete).FirstOrDefaultAsync();
                if (customerPlan is not null)
                {
                    var merchant = await _context.Merchant.Where(x => x.MerchantId == customerPlan.MerchantId && !x.IsDeleted && x.IsActive == true).FirstOrDefaultAsync();
                    var service = await _context.Service.Where(z => z.ServiceId == customerPlan.MasterServiceId && !z.IsDeleted && z.IsActive == true).FirstOrDefaultAsync();
                    if (service is not null)
                    {
                        category = await _context.Category.Where(y => y.CategoryId == service.CategoryId && !y.IsDeleted && y.IsActive == true).FirstOrDefaultAsync();
                        serviceDetails = await _context.ServiceDetails.Where(s => s.ServiceId == service.ServiceId && s.IsActive == true && !s.IsDelete).FirstOrDefaultAsync();
                    }

                    PlanReviewResponse planReviewResponse = new PlanReviewResponse
                    {
                        PlanName = customerPlan.PlanName,
                        PlanImage = customerPlan.PlanImage,
                        Merchant = merchant == null ? "" : merchant.FullName,
                        Category = category == null ? "" : category.CategoryName,
                        Services = service == null ? "" : service.ServiceName,
                        FamilyMembers = serviceDetails == null ? "" : serviceDetails.FamilyMemberName,
                        Frequency = serviceDetails == null ? "" : serviceDetails.PaymentFrequency,
                        ProgressStatics = 100,
                        ScheduleInfos = new List<ScheduleInfo>{
                            new ScheduleInfo
                            {
                                Amount=50,
                                ScheduleDate = DateTime.UtcNow,
                                Status = "Missed"
                            },
                            new ScheduleInfo
                            {
                                Amount=60,
                                ScheduleDate = DateTime.UtcNow,
                                Status = "Paid"
                            },
                            new ScheduleInfo
                            {
                                Amount=70,
                                ScheduleDate = DateTime.UtcNow,
                                Status = "Paid"
                            },
                            new ScheduleInfo
                            {
                                Amount=40,
                                ScheduleDate = DateTime.UtcNow,
                                Status = "Paid"
                            }
                        }
                    };

                    return (true, MessageConstant.CustomerPlanDetailSuccesses, planReviewResponse);
                }
                return (false, MessageConstant.CustomerDataNotFound, null);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Gets the agreement.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="agreementRequest">The agreement request.</param>
        /// <returns></returns>
        public async Task<(bool, string, AgreementResponse)> GetAgreement(long customerId, AgreementRequest agreementRequest)
        {
            AgreementResponse agreementResponse = new AgreementResponse();
            using var trans = await _context.Database.BeginTransactionAsync();

            try
            {
                agreementResponse.Title = "Title";
                agreementResponse.Description = "Description";

                var customerPlans = await _context.CustomerPlans.Where(x => x.CustomerId == customerId && x.MasterServiceId == agreementRequest.ServiceId && !x.IsDelete && x.IsActive == true).FirstOrDefaultAsync();
                if(customerPlans is not null)
                {
                    List<PlanScheduleDeduction> planScheduleDeductionList = await _context.PlanScheduleDeduction.Where(y => y.CustomerPlanId == customerPlans.CustomerPlanId && !y.IsDelete).ToListAsync();
                    if(planScheduleDeductionList != null && planScheduleDeductionList.Count > 0)
                    {
                        SchedulePlanTransactionDetails schedulePlanTransactionDetails = new SchedulePlanTransactionDetails();
                        agreementResponse.SchedulePlanTransactionDetails = new List<SchedulePlanTransactionDetails>();
                        foreach (var planlist in planScheduleDeductionList) {
                            schedulePlanTransactionDetails = new SchedulePlanTransactionDetails();
                            schedulePlanTransactionDetails.ScheduleDate = planlist.PSDDate;
                            schedulePlanTransactionDetails.Amount = planlist.Amount;

                            agreementResponse.SchedulePlanTransactionDetails.Add(schedulePlanTransactionDetails);
                        }
                    }
                }

                return (true, MessageConstant.AgreementSuccessMessage, agreementResponse);
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        #endregion
    }
}