using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels.CustomerPlans.Response;
using HealthLayby.Models.ApiViewModels.Home.Response;
using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// CustomerPlansService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Repositories.IHomeRepository" />
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.ICustomerPlansRepository" />
    public class HomeService : BaseService, IHomeRepository
    {
        #region Private Variable 

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CreditCardService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public HomeService(AppDbContext context) : base(context)
        {
            //  _stripeAppService = stripeAppService;
        }

        #endregion

        #region Mobile API's

        /// <summary>
        /// Gets the dashboard list.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string, List<HomeResponse>)> GetDashboardList(long customerId)
        {
            List<HomeResponse> homeResponsesList = new List<HomeResponse>(); List<CustomerPlans> customerPlanList = new List<CustomerPlans>();
            HomeResponse homeResponses = new HomeResponse(); Merchant merchant = new Merchant();
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
                        homeResponses = new HomeResponse();
                        merchant = _context.Merchant.Where(y => y.MerchantId == list.MerchantId && y.IsDeleted == false && y.IsActive == true).FirstOrDefault();
                        service = _context.Service.Where(z => z.ServiceId == list.MasterServiceId && z.IsDeleted == false && z.IsActive == true).FirstOrDefault();
                        if (service != null)
                            category = _context.Category.Where(xy => xy.CategoryId == service.CategoryId).FirstOrDefault();

                        homeResponses.PlanId = list.CustomerPlanId;
                        homeResponses.PlanName = list.PlanName;
                        homeResponses.PlanServiceName = service == null ? "" : service.ServiceName;
                        homeResponses.PlanCategoryName = category == null ? "" : category.CategoryName;
                        homeResponses.PlanImage = list.PlanImage;
                        homeResponses.PlanName = list.PlanName;
                        homeResponses.IsPlanActive = list.IsActive;
                        homeResponses.PlanEndDate = list.EndDate;

                        homeResponsesList.Add(homeResponses);
                    }
                    message = ApiMessages.SuccessfullDashboardPlanList;
                }
                else
                    message = ApiMessages.NoDashboardPlanList;

                if (homeResponsesList.Count > 0)
                    return (true, message, homeResponsesList);
                else
                    return (false, ApiMessages.InvalidDashboardPlanList, null);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        #endregion
    }
}