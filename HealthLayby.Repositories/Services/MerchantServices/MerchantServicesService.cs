using HealthLayby.Models.Context;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;

namespace HealthLayby.Repositories.Services.MerchantServices
{
    /// <summary>
    /// MerchantServicesService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.MerchantRepositories.IMerchantServicesWebRepository" />
    public class MerchantServicesService : BaseService, IMerchantServicesWebRepository
    {
        #region Private Variable 
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantServicesService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MerchantServicesService(AppDbContext context) : base(context)
        {

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the service details by login.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public async Task<List<MerchantServiceModel>> GetServiceDetailsByLogin(long merchantId)
        {
            try
            {
                List<MerchantServiceModel> merchantServicesList = new List<MerchantServiceModel>(); MerchantServiceModel merchantServiceModel = new MerchantServiceModel();
                var merchant = await _context.Merchant.Where(x => x.MerchantId == merchantId && x.IsActive == true && !x.IsRejected && !x.IsDeleted).FirstOrDefaultAsync();
                if (merchant is not null)
                {
                    List<CustomerPlans> customerPlansList = await _context.CustomerPlans.Where(y => y.MerchantId == merchant.MerchantId && y.IsActive == true && !y.IsDelete).ToListAsync();
                    if (customerPlansList is not null && customerPlansList.Count > 0)
                    {
                        foreach (var item in customerPlansList)
                        {
                            var service = await _context.Service.Where(z => z.ServiceId == item.MasterServiceId && !z.IsDeleted).FirstOrDefaultAsync();
                            if (service is not null)
                            {
                                merchantServiceModel = new MerchantServiceModel();
                                merchantServiceModel.ServiceId = service.ServiceId;
                                merchantServiceModel.ServiceName = service.ServiceName;
                                merchantServiceModel.ServiceDescription = service.Description;
                                merchantServiceModel.ServiceImage = service.Image;
                                merchantServiceModel.ServicePrice = service.Price;
                                merchantServiceModel.ServiceCreatedOn = service.CreatedOn.ToString("MMM dd, yyyy");
                                merchantServiceModel.IsActive = service.IsActive;


                                var category = await _context.Category.Where(i => i.CategoryId == service.CategoryId && i.IsActive == true && !i.IsDeleted).FirstOrDefaultAsync();
                                if (category is not null)
                                    merchantServiceModel.CategoryName = category.CategoryName;

                                merchantServicesList.Add(merchantServiceModel);

                            }
                        }
                    }
                }
                return merchantServicesList;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the service details by serivce identifier.
        /// </summary>
        /// <param name="serviceId">The service identifier.</param>
        /// <returns></returns>
        public async Task<MerchantServiceModel> GetServiceDetailsBySerivceId(long serviceId)
        {
            try
            {
                MerchantServiceModel merchantServiceModel = new MerchantServiceModel();
                var service = await _context.Service.Where(x => x.ServiceId == serviceId && x.IsActive == true && !x.IsDeleted).FirstOrDefaultAsync();
                if (service is not null)
                {
                    merchantServiceModel.ServiceId = service.ServiceId;
                    merchantServiceModel.ServiceName = service.ServiceName;
                    merchantServiceModel.ServiceDescription = service.Description;
                    merchantServiceModel.ServiceImage = service.Image;
                    merchantServiceModel.ServicePrice = service.Price;
                    merchantServiceModel.ServiceCreatedOn = service.CreatedOn.ToString("MMM dd, yyyy");
                    merchantServiceModel.IsActive = service.IsActive;


                    var category = await _context.Category.Where(i => i.CategoryId == service.CategoryId && i.IsActive == true && !i.IsDeleted).FirstOrDefaultAsync();
                    if (category is not null)
                        merchantServiceModel.CategoryName = category.CategoryName;
                }
                return merchantServiceModel;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the service.
        /// </summary>
        /// <param name="merchantServiceModel">The merchant service model.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public async Task<bool> UpdateService(MerchantServiceModel merchantServiceModel, long merchantId)
        {
            try
            {
                var isCheck = await _context.Service.Where(x=>x.ServiceId == merchantServiceModel.ServiceId && x.IsActive == true && !x.IsDeleted).FirstOrDefaultAsync();
                if (isCheck.ServiceId > 0)
                {
                    isCheck.ServiceName = merchantServiceModel.ServiceName;
                    isCheck.Description = merchantServiceModel.ServiceDescription;
                    //isCheck.ServiceImage = merchantServiceModel.ServiceImage;                }
                    isCheck.Price = merchantServiceModel.ServicePrice;
                    isCheck.IsActive = merchantServiceModel.IsActive;
                    isCheck.UpdatedBy = merchantId;
                    isCheck.UpdatedOn = DateTime.UtcNow;

                    _context.Service.Update(isCheck);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                    return false;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the service status.
        /// </summary>
        /// <param name="serviceId">The service identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public async Task<bool> UpdateServiceStatus(long serviceId,long merchantId)
        {
            try
            {
                MerchantServiceModel merchantServiceModel = new MerchantServiceModel();
                var service = await _context.Service.Where(x => x.ServiceId == serviceId && x.IsActive == true && !x.IsDeleted).FirstOrDefaultAsync();
                if (service is not null)
                {
                    service.IsActive = false;
                    service.UpdatedBy = merchantId;
                    service.UpdatedOn = DateTime.UtcNow;

                    _context.Service.Update(service);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                    return false;
            }
            catch 
            {
                throw;
            }
        }

        #endregion
    }
}
