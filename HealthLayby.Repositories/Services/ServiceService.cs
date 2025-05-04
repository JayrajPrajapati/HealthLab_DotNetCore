using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.ApiViewModels.Service.Request;
using HealthLayby.Models.ApiViewModels.Service.Response;
using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Customer = HealthLayby.Models.Models.Customer;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// service service
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.IServiceRepository" />
    /// <seealso cref="Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.ICategoryRepository" />
    public class ServiceService : BaseService, IServiceRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ServiceService(AppDbContext context) : base(context)
        {
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Services the grid list asynchronous.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public async Task<Tuple<List<ServiceGridListResult>, int, int>> ServiceGridListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText)
        {
            try
            {
                var paramTotalRecords = new OutputParameter<int?>();
                var paramTotalFilteredRecords = new OutputParameter<int?>();

                var result = await _context.GetProcedures().ServiceGridListAsync
                (
                    SortColumn: sortColumn,
                    SortOrder: sortOrder,
                    PageSize: pageSize,
                    PageIndex: pageIndex,
                    SearchText: searchText,
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
        /// Saves the service asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveServiceAsync(ServiceModel model, long adminId)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                if (await IsServiceNameAvailableAsync(model.ServiceName, model.ServiceId, model.CategoryId))
                {
                    return (false, MessageConstant.ServiceNameAlreadyExist);
                }

                var isCategoryAvailable = await _context.Category.AnyAsync(q => q.CategoryId == model.CategoryId && !q.IsDeleted);

                if (!isCategoryAvailable)
                {
                    return (false, MessageConstant.CategoryNotFound);
                }

                var message = string.Empty;
                if (model.ServiceId > 0)
                {
                    var service = await _context.Service.FirstOrDefaultAsync(q => q.ServiceId == model.ServiceId && !q.IsDeleted);
                    if (service is null)
                    {
                        return (false, MessageConstant.ServiceNotFound);
                    }

                    //service.MerchantId = model.MerchantId;
                    service.CategoryId = model.CategoryId;
                    service.ServiceName = model.ServiceName;
                    service.Description = model.Description;
                    service.Image = model.Image;
                    service.Price = model.Price;
                    service.IsActive = model.IsActive;
                    service.UpdatedOn = DateTime.UtcNow;
                    service.UpdatedBy = adminId;

                    _context.Service.Update(service);
                    await _context.SaveChangesAsync();
                    message = MessageConstant.UpdateServiceSuccess;
                }
                else
                {
                    await _context.Service.AddAsync(new Service
                    {
                        //MerchantId = model.MerchantId,
                        CategoryId = model.CategoryId,
                        ServiceName = model.ServiceName,
                        Description = model.Description,
                        Image = model.Image,
                        Price = model.Price,
                        IsActive = model.IsActive,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = adminId,
                    });
                    await _context.SaveChangesAsync();
                    message = MessageConstant.CreateServiceSuccess;
                }
                await trans.CommitAsync();
                return (true, message);
            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Gets the service by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<ServiceModel?> GetServiceModelByIdAsync(long id)
        {
            try
            {
                return await (from s in _context.Service
                                  //join m in _context.Merchant on s.MerchantId equals m.MerchantId
                              join c in _context.Category on s.CategoryId equals c.CategoryId
                              where s.ServiceId == id && s.IsDeleted == false
                              orderby s.ServiceId descending
                              select new ServiceModel
                              {
                                  ServiceId = s.ServiceId,
                                  IsActive = s.IsActive,
                                  Image = s.Image,
                                  CategoryId = c.CategoryId,
                                  CategoryName = c.CategoryName,
                                  Price = s.Price,
                                  //MerchantId = m.MerchantId,
                                  //MerchantName = m.FullName,
                                  //MerchantEmail = m.EmailAddress,
                                  //MerchantPhone = m.PhoneNumber,
                                  //MerchantClinicName = m.ClinicName,
                                  ServiceName = s.ServiceName,
                                  Description = s.Description
                              }).LastOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the service aysnc.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> DeleteServiceAysnc(long id, long adminId)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var service = await _context.Service.FirstOrDefaultAsync(x => x.ServiceId == id && !x.IsDeleted);
                if (service is null)
                {
                    return Tuple.Create(false, MessageConstant.ServiceNotFound);
                }

                service.IsDeleted = true;
                service.DeletedBy = adminId;
                service.DeletedOn = DateTime.UtcNow;

                _context.Update(service);
                await _context.SaveChangesAsync();

                await trans.CommitAsync();
                return Tuple.Create(true, MessageConstant.DeleteServiceSuccess);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is service name available] [the specified service name].
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        public async Task<bool> IsServiceNameAvailableAsync(string serviceName, long serviceId, long categoryId)
        {
            try
            {
                return await _context.Service.AnyAsync(x => x.ServiceId != serviceId
                                                        && x.CategoryId == categoryId
                                                        && x.ServiceName.ToLower() == serviceName.ToLower());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the service total count.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetServiceCountAsync()
        {
            try
            {
                return await _context.Service.CountAsync(x => !x.IsDeleted);
            }
            catch
            {
                throw;
            }
        }

        #region For Mobile API's

        /// <summary>
        /// Gets the service list for API.
        /// </summary>
        /// <param name="serviceListingRequest">The service listing request.</param>
        /// <returns></returns>
        public async Task<(bool, string, List<ServiceListingResponse>)> GetServiceListForAPI(ServiceListingRequest serviceListingRequest)
        {
            List<ServiceListingResponse> serviceListingResponseList = new List<ServiceListingResponse>();
            ServiceListingResponse serviceListingResponse = new ServiceListingResponse();
            List<Service> serviceListing = new List<Service>();

            var message = string.Empty;
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                if (serviceListingRequest.sortBy != null && serviceListingRequest.sortBy.ToLower().Contains("desc"))
                {
                    serviceListing = await _context.Service.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.ServiceName).ToListAsync();
                }
                else if (serviceListingRequest.sortBy != null && serviceListingRequest.sortBy.ToLower().Contains("new"))
                {
                    serviceListing = await _context.Service.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.ServiceId).ToListAsync();
                }
                else if (serviceListingRequest.sortBy != null && serviceListingRequest.sortBy.ToLower().Contains("asc"))
                {
                    serviceListing = await _context.Service.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.ServiceName).ToListAsync();
                }
                else if (serviceListingRequest.sortBy != null && serviceListingRequest.sortBy.ToLower().Contains("high"))
                {
                    serviceListing = await _context.Service.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.Price).ToListAsync();
                }
                else if (serviceListingRequest.sortBy != null && serviceListingRequest.sortBy.ToLower().Contains("low"))
                {
                    serviceListing = await _context.Service.Where(x => x.IsActive == true && x.IsDeleted == false).OrderBy(x => x.Price).ToListAsync();
                }
                else
                    serviceListing = await _context.Service.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.ServiceId).ToListAsync();

                if (serviceListing != null)
                {
                    foreach (var list in serviceListing)
                    {
                        serviceListingResponse = new ServiceListingResponse();

                        serviceListingResponse.ServiceId = list.ServiceId;
                        serviceListingResponse.ServiceName = list.ServiceName;
                        serviceListingResponse.ServiceImage = list.Image;
                        serviceListingResponse.Price = list.Price;

                        serviceListingResponseList.Add(serviceListingResponse);
                    }

                    message = ApiMessages.SuccessfullServiceList;

                    if (serviceListingResponseList.Count > 0)
                        return (true, message, serviceListingResponseList);
                    else
                        return (false, ApiMessages.InvalidServiceList, null);
                }
                else
                {
                    return (false, ApiMessages.InvalidServiceList, null);
                }
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Gets the service details for API.
        /// </summary>
        /// <param name="CustomerId">The customer identifier.</param>
        /// <param name="serviceDetailsRequest">The service details request.</param>
        /// <returns></returns>
        public async Task<(bool, string, ServiceDetailsResponse)> GetServiceDetailsForAPI(long CustomerId, ServiceDetailsRequest serviceDetailsRequest)
        {
            var message = string.Empty; ServiceDetails serviceDetails = new ServiceDetails(); ServiceDetails IsserviceDetails = new ServiceDetails(); PlanScheduleDeduction planScheduleDeduction = new PlanScheduleDeduction();
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                Category category = new Category(); Merchant merchant = new Merchant();
                Service service = await _context.Service.Where(x => x.ServiceName == serviceDetailsRequest.ServiceName).FirstOrDefaultAsync();
                CustomerPlans customerPlans = new CustomerPlans();
                if (service is not null)
                {
                    category = await _context.Category.Where(y => y.CategoryId == service.CategoryId).FirstOrDefaultAsync();
                    merchant = await _context.Merchant.Where(z => z.MerchantId == serviceDetailsRequest.MerchentId).FirstOrDefaultAsync();
                }
                if (serviceDetailsRequest.ServiceDetailId == 0)
                {
                    serviceDetails = new ServiceDetails
                    {
                        ServiceId = service != null ? service.ServiceId : 0,
                        Duration = Convert.ToString(serviceDetailsRequest.Duration),
                        Weeks = Convert.ToString(serviceDetailsRequest.Weeks),
                        PaymentFrequency = serviceDetailsRequest.PaymentFrequency,
                        PayAmount = serviceDetailsRequest.PayAmount,
                        StartDate = Convert.ToDateTime(serviceDetailsRequest.StartDate),
                        FamilyMemberName = serviceDetailsRequest.FamilyMember != "" ? serviceDetailsRequest.FamilyMember : null,
                        IsActive = true,
                        CreatedBy = CustomerId,
                        CreatedOn = DateTime.UtcNow,
                        IsDelete = false
                    };

                    await _context.ServiceDetails.AddAsync(serviceDetails);
                    await _context.SaveChangesAsync();
                    message = ApiMessages.SuccessServiceDetail;
                    await trans.CommitAsync();

                    customerPlans = await _context.CustomerPlans.Where(i => i.MasterServiceId == service.ServiceId && i.CustomerId == CustomerId && i.MerchantId == merchant.MerchantId).FirstOrDefaultAsync();

                    #region PSD

                    if (serviceDetailsRequest.Duration > 0) // months
                    {
                        #region Monthly
                        DateTime updatedPSDDate = DateTime.UtcNow;

                        if (serviceDetailsRequest.PaymentFrequency.Contains("Monthly"))
                        {
                            for (int j = 1; j <= serviceDetailsRequest.Duration; j++)
                            {
                                planScheduleDeduction = new PlanScheduleDeduction();
                                planScheduleDeduction.CustomerPlanId = customerPlans == null ? 0 : customerPlans.CustomerPlanId;
                                planScheduleDeduction.PSDDate = j == 1 ? Convert.ToDateTime(serviceDetailsRequest.StartDate) : updatedPSDDate.AddDays(30);
                                planScheduleDeduction.Amount = serviceDetailsRequest.PayAmount;
                                planScheduleDeduction.IsPaid = false;
                                planScheduleDeduction.CreatedBy = CustomerId;
                                planScheduleDeduction.CreatedOn = DateTime.UtcNow;
                                planScheduleDeduction.IsDelete = false;
                                updatedPSDDate = planScheduleDeduction.PSDDate;

                                await _context.PlanScheduleDeduction.AddAsync(planScheduleDeduction);
                                await _context.SaveChangesAsync();

                            }
                            message = ApiMessages.SuccessServiceScheduleDetails;
                        }

                        #endregion

                        #region Weekly
                        else if (serviceDetailsRequest.PaymentFrequency.Contains("Weekly"))
                        {
                            long days = serviceDetailsRequest.Duration * 30;
                            decimal weeks = Math.Round(Convert.ToDecimal(days / 7));

                            for (int j = 1; j <= weeks; j++)
                            {
                                planScheduleDeduction = new PlanScheduleDeduction();
                                planScheduleDeduction.CustomerPlanId = customerPlans == null ? 0 : customerPlans.CustomerPlanId;
                                planScheduleDeduction.PSDDate = j == 1 ? Convert.ToDateTime(serviceDetailsRequest.StartDate) : updatedPSDDate.AddDays(7);
                                planScheduleDeduction.Amount = serviceDetailsRequest.PayAmount;
                                planScheduleDeduction.IsPaid = false;
                                planScheduleDeduction.CreatedBy = CustomerId;
                                planScheduleDeduction.CreatedOn = DateTime.UtcNow;
                                planScheduleDeduction.IsDelete = false;
                                updatedPSDDate = planScheduleDeduction.PSDDate;

                                await _context.PlanScheduleDeduction.AddAsync(planScheduleDeduction);
                                await _context.SaveChangesAsync();
                            }
                            message = ApiMessages.SuccessServiceScheduleDetails;
                        }
                        #endregion
                    }
                    else
                    {
                        #region Weekly
                        DateTime updatedPSDDate = DateTime.UtcNow;

                        for (int j = 1; j <= serviceDetailsRequest.Weeks; j++)
                        {
                            planScheduleDeduction = new PlanScheduleDeduction();
                            planScheduleDeduction.CustomerPlanId = customerPlans == null ? 0 : customerPlans.CustomerPlanId;
                            planScheduleDeduction.PSDDate = j == 1 ? Convert.ToDateTime(serviceDetailsRequest.StartDate) : updatedPSDDate.AddDays(7);
                            planScheduleDeduction.Amount = serviceDetailsRequest.PayAmount;
                            planScheduleDeduction.IsPaid = false;
                            planScheduleDeduction.CreatedBy = CustomerId;
                            planScheduleDeduction.CreatedOn = DateTime.UtcNow;
                            planScheduleDeduction.IsDelete = false;
                            updatedPSDDate = planScheduleDeduction.PSDDate;

                            await _context.PlanScheduleDeduction.AddAsync(planScheduleDeduction);
                            await _context.SaveChangesAsync();
                        }
                        message = ApiMessages.SuccessServiceScheduleDetails;

                        #endregion
                    }

                    #endregion
                }
                else
                {
                    IsserviceDetails = _context.ServiceDetails.Where(y => y.ServiceDetailId == serviceDetailsRequest.ServiceDetailId).FirstOrDefault();
                    if (IsserviceDetails != null && IsserviceDetails.ServiceDetailId > 0)
                    {
                        IsserviceDetails.Duration = Convert.ToString(serviceDetailsRequest.Duration);
                        IsserviceDetails.Weeks = Convert.ToString(serviceDetailsRequest.Weeks);
                        IsserviceDetails.PaymentFrequency = serviceDetailsRequest.PaymentFrequency;
                        IsserviceDetails.PayAmount = serviceDetailsRequest.PayAmount;
                        IsserviceDetails.UpdatedBy = CustomerId;
                        IsserviceDetails.UpdatedOn = DateTime.UtcNow;

                        _context.ServiceDetails.Update(IsserviceDetails);
                        await _context.SaveChangesAsync();
                        message = ApiMessages.UpdateServiceDetail;

                        await trans.CommitAsync();

                        customerPlans = await _context.CustomerPlans.Where(i => i.MasterServiceId == service.ServiceId && i.CustomerId == CustomerId && i.MerchantId == merchant.MerchantId).FirstOrDefaultAsync();
                        if (customerPlans is not null)
                        {
                            List<PlanScheduleDeduction> planScheduleDeductionList = await _context.PlanScheduleDeduction.Where(x => x.CustomerPlanId == customerPlans.CustomerPlanId && !x.IsPaid && !x.IsDelete).ToListAsync();
                            if (planScheduleDeductionList != null && planScheduleDeductionList.Count > 0)
                            {
                                foreach (var psdList in planScheduleDeductionList)
                                {
                                    psdList.IsDelete = true;
                                    psdList.UpdatedBy = CustomerId;
                                    psdList.UpdatedOn = DateTime.UtcNow;

                                    _context.PlanScheduleDeduction.Update(psdList);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                        #region PSD

                        if (serviceDetailsRequest.Duration > 0)
                        {
                            #region Monthly
                            DateTime updatedPSDDate = DateTime.UtcNow;

                            if (serviceDetailsRequest.PaymentFrequency.Contains("Monthly"))
                            {
                                for (int j = 1; j <= serviceDetailsRequest.Duration; j++)
                                {
                                    planScheduleDeduction = new PlanScheduleDeduction();
                                    planScheduleDeduction.CustomerPlanId = customerPlans == null ? 0 : customerPlans.CustomerPlanId;
                                    planScheduleDeduction.PSDDate = j == 1 ? Convert.ToDateTime(serviceDetailsRequest.StartDate) : updatedPSDDate.AddDays(30);
                                    planScheduleDeduction.Amount = serviceDetailsRequest.PayAmount;
                                    planScheduleDeduction.IsPaid = false;
                                    planScheduleDeduction.CreatedBy = CustomerId;
                                    planScheduleDeduction.CreatedOn = DateTime.UtcNow;
                                    planScheduleDeduction.IsDelete = false;
                                    updatedPSDDate = planScheduleDeduction.PSDDate;

                                    await _context.PlanScheduleDeduction.AddAsync(planScheduleDeduction);
                                    await _context.SaveChangesAsync();

                                }
                                message = ApiMessages.UpdateServiceScheduleDetails;
                            }

                            #endregion

                            #region Weekly
                            else if (serviceDetailsRequest.PaymentFrequency.Contains("Weekly"))
                            {
                                long days = serviceDetailsRequest.Duration * 30;
                                decimal weeks = Math.Round(Convert.ToDecimal(days / 7));

                                for (int j = 1; j <= weeks; j++)
                                {
                                    planScheduleDeduction = new PlanScheduleDeduction();
                                    planScheduleDeduction.CustomerPlanId = customerPlans == null ? 0 : customerPlans.CustomerPlanId;
                                    planScheduleDeduction.PSDDate = j == 1 ? Convert.ToDateTime(serviceDetailsRequest.StartDate) : updatedPSDDate.AddDays(7);
                                    planScheduleDeduction.Amount = serviceDetailsRequest.PayAmount;
                                    planScheduleDeduction.IsPaid = false;
                                    planScheduleDeduction.CreatedBy = CustomerId;
                                    planScheduleDeduction.CreatedOn = DateTime.UtcNow;
                                    planScheduleDeduction.IsDelete = false;
                                    updatedPSDDate = planScheduleDeduction.PSDDate;

                                    await _context.PlanScheduleDeduction.AddAsync(planScheduleDeduction);
                                    await _context.SaveChangesAsync();
                                }
                                message = ApiMessages.UpdateServiceScheduleDetails;
                            }
                            #endregion
                        }
                        else
                        {
                            #region Weekly
                            DateTime updatedPSDDate = DateTime.UtcNow;

                            for (int j = 1; j <= serviceDetailsRequest.Weeks; j++)
                            {
                                planScheduleDeduction = new PlanScheduleDeduction();
                                planScheduleDeduction.CustomerPlanId = customerPlans == null ? 0 : customerPlans.CustomerPlanId;
                                planScheduleDeduction.PSDDate = j == 1 ? Convert.ToDateTime(serviceDetailsRequest.StartDate) : updatedPSDDate.AddDays(7);
                                planScheduleDeduction.Amount = serviceDetailsRequest.PayAmount;
                                planScheduleDeduction.IsPaid = false;
                                planScheduleDeduction.CreatedBy = CustomerId;
                                planScheduleDeduction.CreatedOn = DateTime.UtcNow;
                                planScheduleDeduction.IsDelete = false;
                                updatedPSDDate = planScheduleDeduction.PSDDate;

                                await _context.PlanScheduleDeduction.AddAsync(planScheduleDeduction);
                                await _context.SaveChangesAsync();
                            }
                            message = ApiMessages.UpdateServiceScheduleDetails;

                            #endregion
                        }

                        #endregion
                    }
                }

                ServiceDetailsResponse serviceDetailsResponse = new ServiceDetailsResponse()
                {
                    ServiceDetailsId = serviceDetails.ServiceDetailId == 0 ? IsserviceDetails.ServiceDetailId : serviceDetails.ServiceDetailId,
                    ServiceId = service == null ? 0 : service.ServiceId,
                    ServiceName = service != null ? service.ServiceName : "",
                    CategoryName = category == null ? "" : category.CategoryName,
                    MerchentName = merchant == null ? "" : merchant.FullName,
                    MerchantImage = merchant == null ? "" : merchant.ProfilePic,
                    ServicePrice = service != null ? service.Price : 0,
                    Duration = Convert.ToInt32(serviceDetails.Duration) == 0 ? Convert.ToInt32(IsserviceDetails.Duration) : Convert.ToInt32(serviceDetails.Duration),
                    Weeks = Convert.ToInt32(serviceDetails.Weeks) == 0 ? Convert.ToInt32(IsserviceDetails.Weeks) : Convert.ToInt32(serviceDetails.Weeks),
                    PaymentFrequent = serviceDetails.PaymentFrequency == null ? IsserviceDetails.PaymentFrequency : serviceDetails.PaymentFrequency,
                    PayAmount = serviceDetails.PayAmount == null ? IsserviceDetails.PayAmount : serviceDetails.PayAmount,
                    FamilyMember = serviceDetails.FamilyMemberName == null ? "" : serviceDetails.FamilyMemberName
                };
                if (serviceDetailsResponse.ServiceDetailsId > 0)
                    return (true, message, serviceDetailsResponse);
                else
                    return (false, ApiMessages.InvalidServiceDetail, null);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        #endregion

        #region Merchent

        /// <summary>
        /// Gets the service list for dashboard.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns></returns>
        public async Task<List<Service>> GetServiceListForDashboard(string firstName)
        {
            List<Service> servicesList = new List<Service>(); Service service = new Service();
            string ids = string.Empty;
            var merchant = await _context.Merchant.Where(y => y.FirstName == firstName).FirstOrDefaultAsync();
            if (merchant is not null)
            {
                List<CustomerPlans> customerPlan = await _context.CustomerPlans.Where(z => z.MerchantId == merchant.MerchantId).ToListAsync();
                if (customerPlan.Count > 0)
                {
                    foreach (var i in customerPlan)
                    {
                        service = await _context.Service.Where(x => x.ServiceId == i.MasterServiceId && x.IsActive == true && !x.IsDeleted).FirstOrDefaultAsync();
                        if (service is not null)
                        {
                            if (ids == null || ids == "")
                                ids = Convert.ToString(service.ServiceId);
                            else
                                ids = ids + "," + Convert.ToString(service.ServiceId);
                        }
                    }
                    servicesList = await _context.Service.Where(x => x.IsActive == true && !x.IsDeleted && ids.Contains(Convert.ToString(x.ServiceId))).OrderByDescending(x => x.Price).ToListAsync();
                }
            }
            return servicesList;
        }

        /// <summary>
        /// Gets the transaction list for dashboard.
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <returns></returns>
        public async Task<List<TempLayByPlan>> GetTransactionListForDashboard(string firstName)
        {
            List<TempLayByPlan> tempLayByPlansList = new List<TempLayByPlan>();
            var merchant = await _context.Merchant.Where(y => y.FirstName == firstName).FirstOrDefaultAsync();
            if (merchant is not null)
            {
                List<CustomerPlans> customerPlan = await _context.CustomerPlans.Where(z => z.MerchantId == merchant.MerchantId).ToListAsync();
                if (customerPlan.Count > 0)
                {
                    foreach (var i in customerPlan)
                    {
                        Customer customer = await _context.Customer.Where(i => i.CustomerId == i.CustomerId && i.IsActive == true && !i.IsDeleted).FirstOrDefaultAsync();
                        if (customer is not null)
                        {
                            tempLayByPlansList = await _context.TempLayByPlan.Where(x => x.Customer == customer.FullName && x.IsDeleted == false).ToListAsync();
                        }
                    }
                }
            }
            return tempLayByPlansList;
        }

        #endregion


        #endregion
    }
}