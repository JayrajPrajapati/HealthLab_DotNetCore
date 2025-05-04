using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.Context;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Drawing.Printing;
using System.Numerics;
using Customer = HealthLayby.Models.Models.Customer;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// LayBy Service
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.ILayByRepository" />
    public class LayByService : BaseService, ILayByRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LayByService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public LayByService(AppDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the lay by list asynchronous.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public async Task<Tuple<List<LayByPlanListResult>, int, int>> GetLayByListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText, long status)
        {
            try
            {
                var paramTotalRecords = new OutputParameter<int?>();
                var paramTotalFilteredRecords = new OutputParameter<int?>();

                var result = await _context.GetProcedures().LayByPlanListAsync
                (
                    SortColumn: sortColumn,
                    SortOrder: sortOrder,
                    PageSize: pageSize,
                    PageIndex: pageIndex,
                    SearchText: searchText,
                    Status: status,
                    TotalRecords: paramTotalRecords,
                    TotalFilteredRecords: paramTotalFilteredRecords
                );

                return Tuple.Create(result, paramTotalRecords.Value ?? 0, paramTotalFilteredRecords.Value ?? 0);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the lay by by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<LayByModel?> GetLayByByIdAsync(long id)
        {
            CustomerPlans customerPlans = new CustomerPlans();
            Service service = new Service();
            Category category = new Category();
            Merchant merchant = new Merchant();
            ServiceDetails serviceDetails = new ServiceDetails();
            LayByModel layByModel = new LayByModel();
            Customer customer = new Customer();
            try
            {
                if (id > 0)
                {
                    customerPlans = await _context.CustomerPlans.FirstOrDefaultAsync(x => x.CustomerPlanId == id && !x.IsDelete && x.IsActive == true);
                    if (customerPlans is not null)
                    {
                        service = await _context.Service.FirstOrDefaultAsync(x => x.ServiceId == customerPlans.MasterServiceId && !x.IsDeleted && x.IsActive == true);
                        if (service is not null)
                        {
                            serviceDetails = await _context.ServiceDetails.FirstOrDefaultAsync(x => x.ServiceId == service.ServiceId && !x.IsDelete && x.IsActive == true);
                            if (serviceDetails is not null)
                            {
                                layByModel.Duration = serviceDetails.Duration;
                                layByModel.Frequency = serviceDetails.PaymentFrequency;
                                layByModel.PayAmount = serviceDetails.PayAmount;
                            }
                            category = await _context.Category.FirstOrDefaultAsync(x => x.CategoryId == service.CategoryId && !x.IsDeleted && x.IsActive == true);
                            if (category is not null)
                            {
                                layByModel.CategoryName = category.CategoryName;
                                layByModel.CategoryImage = category.Image;
                            }
                            layByModel.ServiceName = service.ServiceName;
                        }

                        merchant = await _context.Merchant.FirstOrDefaultAsync(x => x.MerchantId == customerPlans.MerchantId && !x.IsDeleted && x.IsActive == true);
                        if (merchant != null)
                        {
                            layByModel.MerchantName = merchant.FullName;
                            layByModel.MerchantEmail = merchant.EmailAddress;
                            layByModel.MerchantPhoneNumber = merchant.PhoneNumber;
                            layByModel.MerchantImage = merchant.ProfilePic;
                        }

                        customer = await _context.Customer.FirstOrDefaultAsync(x => x.CustomerId == customerPlans.CustomerId && !x.IsDeleted && x.IsActive == true);
                        if (customer != null)
                        {
                            layByModel.CustomerName = customer.FullName;
                            layByModel.CustomerEmail = customer.EmailAddress;
                            layByModel.CustomerPhoneNumber = customer.PhoneNumber;
                        }

                        var nextPlanDeduction = await _context.PlanScheduleDeduction.Where(v => v.CustomerPlanId == customerPlans.CustomerPlanId && !v.IsPaid && !v.IsDelete).FirstOrDefaultAsync();
                        if (nextPlanDeduction is not null)
                        {
                            layByModel.NextDeductionDate = nextPlanDeduction.PSDDate;
                            layByModel.NextAmountDeduction = nextPlanDeduction.Amount;
                        }

                        var paidPlanDeduction = await _context.PlanScheduleDeduction.Where(v => v.CustomerPlanId == customerPlans.CustomerPlanId && v.IsPaid == true && !v.IsDelete).ToListAsync();
                        if (paidPlanDeduction.Count > 0)
                        {
                            layByModel.TotalAmountReceived = paidPlanDeduction.Count > 0 ? paidPlanDeduction.Sum(x => x.Amount) : 0;
                        }

                    }
                }
                return layByModel;
            }
            catch
            {

                throw;
            }
        }
    }
}
