using HealthLayby.Models.Context;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Stripe;
using Customer = HealthLayby.Models.Models.Customer;

namespace HealthLayby.Repositories.Services.MerchantServices
{
    /// <summary>
    /// MerchantPlansService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.MerchantRepositories.IMerchantPlansWebRepository" />
    public class MerchantPlansService : BaseService, IMerchantPlansWebRepository
    {
        #region Private Variable 
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantServicesService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MerchantPlansService(AppDbContext context) : base(context)
        {
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the plans list.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public async Task<List<MerchantPlansModel>> GetAllPlansList(long merchantId)
        {
            try
            {
                List<MerchantPlansModel> merchantPlansModelList = new List<MerchantPlansModel>();
                MerchantPlansModel merchantPlansModel = new MerchantPlansModel(); decimal totalAmtPaid = 0;

                var plansList = await _context.CustomerPlans.Where(x => x.MerchantId == merchantId && !x.IsDelete).ToListAsync();
                if (plansList.Count > 0)
                {
                    foreach (var item in plansList)
                    {
                        merchantPlansModel = new MerchantPlansModel();
                        merchantPlansModel.CustomerPlanId = item.CustomerPlanId;

                        if (item.IsActive == true)
                        {
                            if (item.Status == 1)
                                merchantPlansModel.PlanStatus = "Completed";
                            else if (item.Status == 2)
                                merchantPlansModel.PlanStatus = "Pause";
                            else if (item.Status == 3)
                                merchantPlansModel.PlanStatus = "Cancelled";
                            else if (item.Status == 0)
                                merchantPlansModel.PlanStatus = "Active";
                        }

                        var paidPlanDeduction = await _context.PlanScheduleDeduction.Where(v => v.CustomerPlanId == item.CustomerPlanId && v.IsPaid == true && !v.IsDelete).ToListAsync();
                        if (paidPlanDeduction.Count > 0)
                        {
                            merchantPlansModel.CustomerTotalAmtPaid = paidPlanDeduction.Sum(y => y.Amount);
                        }

                        var customer = await _context.Customer.Where(y => y.CustomerId == item.CustomerId && y.IsActive == true && !y.IsDeleted).FirstOrDefaultAsync();
                        if (customer is not null)
                        {
                            merchantPlansModel.CustomerName = customer.FullName;
                            merchantPlansModel.CustomerPhoneNumber = customer.PhoneNumber;

                            var merchant = await _context.Merchant.Where(z => z.MerchantId == item.MerchantId && z.IsActive == true && !z.IsDeleted).FirstOrDefaultAsync();
                            if (merchant is not null)
                            {
                                merchantPlansModel.MerchantName = merchant.FullName;
                            }

                            var service = await _context.Service.Where(i => i.ServiceId == item.MasterServiceId && i.IsActive == true && !i.IsDeleted).FirstOrDefaultAsync();
                            if (service is not null)
                            {
                                merchantPlansModel.ServiceName = service.ServiceName;
                                merchantPlansModel.ServiceAmount = service.Price;

                                var serviceDetails = await _context.ServiceDetails.Where(j => j.ServiceId == service.ServiceId && j.IsActive == true && !j.IsDelete).FirstOrDefaultAsync();
                                if (serviceDetails is not null)
                                {
                                    merchantPlansModel.ServiceDuration = serviceDetails.Duration;
                                    merchantPlansModel.ServiceFrequency = serviceDetails.PaymentFrequency;
                                }
                            }
                        }
                        merchantPlansModelList.Add(merchantPlansModel);
                    }
                }
                return merchantPlansModelList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the active plans list.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public async Task<List<MerchantPlansModel>> GetActivePlansList(long merchantId)
        {
            try
            {
                List<MerchantPlansModel> merchantPlansModelList = new List<MerchantPlansModel>();
                MerchantPlansModel merchantPlansModel = new MerchantPlansModel();

                var plansList = await _context.CustomerPlans.Where(x => x.MerchantId == merchantId && x.IsActive == true && x.Status == 0 && !x.IsDelete).ToListAsync();
                if (plansList.Count > 0)
                {
                    foreach (var item in plansList)
                    {
                        merchantPlansModel = new MerchantPlansModel();
                        merchantPlansModel.CustomerPlanId = item.CustomerPlanId;

                        if (item.IsActive == true && item.Status == 0)
                            merchantPlansModel.PlanStatus = "Active";

                        var paidPlanDeduction = await _context.PlanScheduleDeduction.Where(v => v.CustomerPlanId == item.CustomerPlanId && v.IsPaid == true && !v.IsDelete).ToListAsync();
                        if (paidPlanDeduction.Count > 0)
                        {
                            merchantPlansModel.CustomerTotalAmtPaid = paidPlanDeduction.Sum(y => y.Amount);
                        }

                        var customer = await _context.Customer.Where(y => y.CustomerId == item.CustomerId && y.IsActive == true && !y.IsDeleted).FirstOrDefaultAsync();
                        if (customer is not null)
                        {
                            merchantPlansModel.CustomerName = customer.FullName;
                            merchantPlansModel.CustomerPhoneNumber = customer.PhoneNumber;

                            var merchant = await _context.Merchant.Where(z => z.MerchantId == item.MerchantId && z.IsActive == true && !z.IsDeleted).FirstOrDefaultAsync();
                            if (merchant is not null)
                            {
                                merchantPlansModel.MerchantName = merchant.FullName;
                            }

                            var service = await _context.Service.Where(i => i.ServiceId == item.MasterServiceId && i.IsActive == true && !i.IsDeleted).FirstOrDefaultAsync();
                            if (service is not null)
                            {
                                merchantPlansModel.ServiceName = service.ServiceName;
                                merchantPlansModel.ServiceAmount = service.Price;

                                var serviceDetails = await _context.ServiceDetails.Where(j => j.ServiceId == service.ServiceId && j.IsActive == true && !j.IsDelete).FirstOrDefaultAsync();
                                if (serviceDetails is not null)
                                {
                                    merchantPlansModel.ServiceDuration = serviceDetails.Duration;
                                    merchantPlansModel.ServiceFrequency = serviceDetails.PaymentFrequency;
                                }
                            }
                        }
                        merchantPlansModelList.Add(merchantPlansModel);
                    }
                }
                return merchantPlansModelList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the completed plans list.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public async Task<List<MerchantPlansModel>> GetCompletedPlansList(long merchantId)
        {
            try
            {
                List<MerchantPlansModel> merchantPlansModelList = new List<MerchantPlansModel>();
                MerchantPlansModel merchantPlansModel = new MerchantPlansModel();

                var plansList = await _context.CustomerPlans.Where(x => x.MerchantId == merchantId && x.IsActive == true && x.Status == 1 && !x.IsDelete).ToListAsync();
                if (plansList.Count > 0)
                {
                    foreach (var item in plansList)
                    {
                        merchantPlansModel = new MerchantPlansModel();
                        merchantPlansModel.CustomerPlanId = item.CustomerPlanId;

                        if (item.IsActive == true && item.Status == 1)
                            merchantPlansModel.PlanStatus = "Completed";

                        var paidPlanDeduction = await _context.PlanScheduleDeduction.Where(v => v.CustomerPlanId == item.CustomerPlanId && v.IsPaid == true && !v.IsDelete).ToListAsync();
                        if (paidPlanDeduction.Count > 0)
                        {
                            merchantPlansModel.CustomerTotalAmtPaid = paidPlanDeduction.Sum(y => y.Amount);
                        }

                        var customer = await _context.Customer.Where(y => y.CustomerId == item.CustomerId && y.IsActive == true && !y.IsDeleted).FirstOrDefaultAsync();
                        if (customer is not null)
                        {
                            merchantPlansModel.CustomerName = customer.FullName;
                            merchantPlansModel.CustomerPhoneNumber = customer.PhoneNumber;

                            var merchant = await _context.Merchant.Where(z => z.MerchantId == item.MerchantId && z.IsActive == true && !z.IsDeleted).FirstOrDefaultAsync();
                            if (merchant is not null)
                            {
                                merchantPlansModel.MerchantName = merchant.FullName;
                            }

                            var service = await _context.Service.Where(i => i.ServiceId == item.MasterServiceId && i.IsActive == true && !i.IsDeleted).FirstOrDefaultAsync();
                            if (service is not null)
                            {
                                merchantPlansModel.ServiceName = service.ServiceName;
                                merchantPlansModel.ServiceAmount = service.Price;

                                var serviceDetails = await _context.ServiceDetails.Where(j => j.ServiceId == service.ServiceId && j.IsActive == true && !j.IsDelete).FirstOrDefaultAsync();
                                if (serviceDetails is not null)
                                {
                                    merchantPlansModel.ServiceDuration = serviceDetails.Duration;
                                    merchantPlansModel.ServiceFrequency = serviceDetails.PaymentFrequency;
                                }
                            }
                        }
                        merchantPlansModelList.Add(merchantPlansModel);
                    }
                }
                return merchantPlansModelList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the pause plans list.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public async Task<List<MerchantPlansModel>> GetPausePlansList(long merchantId)
        {
            try
            {
                List<MerchantPlansModel> merchantPlansModelList = new List<MerchantPlansModel>();
                MerchantPlansModel merchantPlansModel = new MerchantPlansModel();

                var plansList = await _context.CustomerPlans.Where(x => x.MerchantId == merchantId && x.IsActive == true && x.Status == 2 && !x.IsDelete).ToListAsync();
                if (plansList.Count > 0)
                {
                    foreach (var item in plansList)
                    {
                        merchantPlansModel = new MerchantPlansModel();
                        merchantPlansModel.CustomerPlanId = item.CustomerPlanId;

                        if (item.IsActive == true && item.Status == 2)
                            merchantPlansModel.PlanStatus = "Pause";

                        var paidPlanDeduction = await _context.PlanScheduleDeduction.Where(v => v.CustomerPlanId == item.CustomerPlanId && v.IsPaid == true && !v.IsDelete).ToListAsync();
                        if (paidPlanDeduction.Count > 0)
                        {
                            merchantPlansModel.CustomerTotalAmtPaid = paidPlanDeduction.Sum(y => y.Amount);
                        }

                        var customer = await _context.Customer.Where(y => y.CustomerId == item.CustomerId && y.IsActive == true && !y.IsDeleted).FirstOrDefaultAsync();
                        if (customer is not null)
                        {
                            merchantPlansModel.CustomerName = customer.FullName;
                            merchantPlansModel.CustomerPhoneNumber = customer.PhoneNumber;

                            var merchant = await _context.Merchant.Where(z => z.MerchantId == item.MerchantId && z.IsActive == true && !z.IsDeleted).FirstOrDefaultAsync();
                            if (merchant is not null)
                            {
                                merchantPlansModel.MerchantName = merchant.FullName;
                            }

                            var service = await _context.Service.Where(i => i.ServiceId == item.MasterServiceId && i.IsActive == true && !i.IsDeleted).FirstOrDefaultAsync();
                            if (service is not null)
                            {
                                merchantPlansModel.ServiceName = service.ServiceName;
                                merchantPlansModel.ServiceAmount = service.Price;

                                var serviceDetails = await _context.ServiceDetails.Where(j => j.ServiceId == service.ServiceId && j.IsActive == true && !j.IsDelete).FirstOrDefaultAsync();
                                if (serviceDetails is not null)
                                {
                                    merchantPlansModel.ServiceDuration = serviceDetails.Duration;
                                    merchantPlansModel.ServiceFrequency = serviceDetails.PaymentFrequency;
                                }
                            }
                        }
                        merchantPlansModelList.Add(merchantPlansModel);
                    }
                }
                return merchantPlansModelList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the cancel plans list.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public async Task<List<MerchantPlansModel>> GetCancelPlansList(long merchantId)
        {
            try
            {
                List<MerchantPlansModel> merchantPlansModelList = new List<MerchantPlansModel>();
                MerchantPlansModel merchantPlansModel = new MerchantPlansModel();

                var plansList = await _context.CustomerPlans.Where(x => x.MerchantId == merchantId && x.IsActive == true && x.Status == 3 && !x.IsDelete).ToListAsync();
                if (plansList.Count > 0)
                {
                    foreach (var item in plansList)
                    {
                        merchantPlansModel = new MerchantPlansModel();
                        merchantPlansModel.CustomerPlanId = item.CustomerPlanId;

                        if (item.IsActive == true && item.Status == 3)
                            merchantPlansModel.PlanStatus = "Cancelled";

                        var paidPlanDeduction = await _context.PlanScheduleDeduction.Where(v => v.CustomerPlanId == item.CustomerPlanId && v.IsPaid == true && !v.IsDelete).ToListAsync();
                        if (paidPlanDeduction.Count > 0)
                        {
                            merchantPlansModel.CustomerTotalAmtPaid = paidPlanDeduction.Sum(y => y.Amount);
                        }

                        var customer = await _context.Customer.Where(y => y.CustomerId == item.CustomerId && y.IsActive == true && !y.IsDeleted).FirstOrDefaultAsync();
                        if (customer is not null)
                        {
                            merchantPlansModel.CustomerName = customer.FullName;
                            merchantPlansModel.CustomerPhoneNumber = customer.PhoneNumber;

                            var merchant = await _context.Merchant.Where(z => z.MerchantId == item.MerchantId && z.IsActive == true && !z.IsDeleted).FirstOrDefaultAsync();
                            if (merchant is not null)
                            {
                                merchantPlansModel.MerchantName = merchant.FullName;
                            }

                            var service = await _context.Service.Where(i => i.ServiceId == item.MasterServiceId && i.IsActive == true && !i.IsDeleted).FirstOrDefaultAsync();
                            if (service is not null)
                            {
                                merchantPlansModel.ServiceName = service.ServiceName;
                                merchantPlansModel.ServiceAmount = service.Price;

                                var serviceDetails = await _context.ServiceDetails.Where(j => j.ServiceId == service.ServiceId && j.IsActive == true && !j.IsDelete).FirstOrDefaultAsync();
                                if (serviceDetails is not null)
                                {
                                    merchantPlansModel.ServiceDuration = serviceDetails.Duration;
                                    merchantPlansModel.ServiceFrequency = serviceDetails.PaymentFrequency;
                                }
                            }
                        }
                        merchantPlansModelList.Add(merchantPlansModel);
                    }
                }
                return merchantPlansModelList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the plans details by identifier.
        /// </summary>
        /// <param name="customerPlanId">The customer plan identifier.</param>
        /// <returns></returns>
        public async Task<MerchantPlansModel> GetPlansDetailsById(long customerPlanId)
        {
            try
            {
                MerchantPlansModel merchantPlansModel = new MerchantPlansModel();

                var plan = await _context.CustomerPlans.Where(x => x.CustomerPlanId == customerPlanId && !x.IsDelete).FirstOrDefaultAsync();
                if (plan is not null)
                {
                    merchantPlansModel.CustomerPlanId = customerPlanId;
                    if (plan.IsActive == true)
                    {
                        if (plan.Status == 1)
                            merchantPlansModel.PlanStatus = "Completed";
                        else if (plan.Status == 2)
                            merchantPlansModel.PlanStatus = "Pause";
                        else if (plan.Status == 3)
                            merchantPlansModel.PlanStatus = "Cancelled";
                        else if (plan.Status == 0)
                            merchantPlansModel.PlanStatus = "Active";
                    }

                    var nextPlanDeduction = await _context.PlanScheduleDeduction.Where(v => v.CustomerPlanId == plan.CustomerPlanId && !v.IsPaid && !v.IsDelete).FirstOrDefaultAsync();
                    if (nextPlanDeduction is not null)
                    {
                        merchantPlansModel.CustomerNextDeductionDate = nextPlanDeduction.PSDDate.ToString("MMM dd,yyyy");
                        merchantPlansModel.CustomerNextAmt = nextPlanDeduction.Amount;
                    }
                    var paidPlanDeduction = await _context.PlanScheduleDeduction.Where(v => v.CustomerPlanId == plan.CustomerPlanId && v.IsPaid == true && !v.IsDelete).ToListAsync();
                    if (paidPlanDeduction.Count > 0)
                    {
                        merchantPlansModel.CustomerTotalAmtPaid = paidPlanDeduction.Count > 0 ? paidPlanDeduction.Sum(x => x.Amount) : 0;
                    }

                    var customer = await _context.Customer.Where(y => y.CustomerId == plan.CustomerId && y.IsActive == true && !y.IsDeleted).FirstOrDefaultAsync();
                    if (customer is not null)
                    {
                        merchantPlansModel.CustomerName = customer.FullName;
                        merchantPlansModel.CustomerPhoneNumber = customer.PhoneNumber;
                        merchantPlansModel.CustomerEmail = customer.EmailAddress;
                        merchantPlansModel.CustomerStatus = customer.IsActive == true ? "Active" : "In-Active";

                        var merchant = await _context.Merchant.Where(z => z.MerchantId == plan.MerchantId && z.IsActive == true && !z.IsDeleted).FirstOrDefaultAsync();
                        if (merchant is not null)
                        {
                            merchantPlansModel.MerchantName = merchant.FullName;
                        }

                        var service = await _context.Service.Where(i => i.ServiceId == plan.MasterServiceId && i.IsActive == true && !i.IsDeleted).FirstOrDefaultAsync();
                        if (service is not null)
                        {
                            merchantPlansModel.ServiceName = service.ServiceName;
                            merchantPlansModel.ServiceAmount = service.Price;

                            var category = await _context.Category.Where(q => q.CategoryId == service.CategoryId && q.IsActive == true && !q.IsDeleted).FirstOrDefaultAsync();
                            if (category is not null)
                                merchantPlansModel.ServiceCategoryName = category.CategoryName;

                            var serviceDetails = await _context.ServiceDetails.Where(j => j.ServiceId == service.ServiceId && j.IsActive == true && !j.IsDelete).FirstOrDefaultAsync();
                            if (serviceDetails is not null)
                            {
                                merchantPlansModel.ServiceDuration = serviceDetails.Duration;
                                merchantPlansModel.ServiceFrequency = serviceDetails.PaymentFrequency;
                            }
                        }
                    }
                }
                return merchantPlansModel;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is merchant exists] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        public async Task<Merchant> IsMerchantExists(string fullName)
        {
            var isMerchantExistsRejected = await _context.Merchant.Where(x => x.FullName == fullName && x.IsRejected == true).FirstOrDefaultAsync();

            if (isMerchantExistsRejected is not null)
                return isMerchantExistsRejected;
            else
            {
                isMerchantExistsRejected = await _context.Merchant.Where(x => x.FullName == fullName && !x.IsDeleted).FirstOrDefaultAsync();
                if (isMerchantExistsRejected is not null)
                    return isMerchantExistsRejected;
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets the customer details.
        /// </summary>
        /// <param name="customerName">Name of the customer.</param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerDetails(string customerName)
        {
            try
            {
                var customer = await _context.Customer.Where(x => x.FullName == customerName && x.IsActive == true && !x.IsDeleted).FirstOrDefaultAsync();
                return customer;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the plan.
        /// </summary>
        /// <param name="customerPlanId">The customer plan identifier.</param>
        /// <param name="submittedOTP">The submitted otp.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public async Task<bool> UpdatePlan(long customerPlanId, long submittedOTP, long merchantId)
        {
            var validateOTP = await _context.GenerateOTP.Where(y => y.Otp == submittedOTP && !y.IsUsed).FirstOrDefaultAsync();
            if (validateOTP is not null)
            {
                var model = await _context.CustomerPlans.Where(x => x.CustomerPlanId == customerPlanId && x.IsActive == true && !x.IsDelete).FirstOrDefaultAsync();
                if (model is not null)
                {
                    model.Status = 1;
                    model.UpdatedBy = merchantId;
                    model.UpdatedOn = DateTime.UtcNow;

                    _context.CustomerPlans.Update(model);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        #endregion
    }
}
