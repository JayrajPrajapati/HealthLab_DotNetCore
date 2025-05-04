using HealthLayby.Helpers.Constant;
using HealthLayby.Models.Context;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.EntityFrameworkCore;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Repositories.Services.MerchantServices
{
    /// <summary>
    /// MerchantTermsConditionService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.MerchantRepositories.IMerchantTermsWebRepository" />
    public class MerchantTermsConditionService : BaseService, IMerchantTermsWebRepository
    {
        #region Private Variable 
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantTermsConditionService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MerchantTermsConditionService(AppDbContext context) : base(context)
        {

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the merchant terms.
        /// </summary>
        /// <returns></returns>
        public async Task<MerchantTermsConditionModel> GetMerchantTerms()
        {
            try
            {
                return await _context.ContentManagement.Where(q => q.PageCode == 2 && q.IsActive == true)
                                            .Select(q => new MerchantTermsConditionModel
                                            {
                                                ContentManagementId = q.ContentManagementId,
                                                Title = q.Title,
                                                Description = q.Description
                                            }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Saves the terms condition asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveTermsConditionAsync(TermsConditionModel model, long merchantId)
        {
            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (model.IsActive)
                    {
                        if (string.IsNullOrWhiteSpace(model.Description))
                        {
                            return (true, MessageConstant.TermsInValid);
                        }
                    }

                    var contentManagement = await _context.ContentManagement.FirstOrDefaultAsync(x => x.PageCode == (int)model.PageCode);
                    if (contentManagement is null)
                    {
                        contentManagement = new ContentManagement
                        {
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = merchantId,
                            PageCode = (int)model.PageCode,
                            Description = model.Description,
                            IsActive = model.IsActive,
                            Title = model.Title
                        };

                        await _context.ContentManagement.AddAsync(contentManagement);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        contentManagement.UpdatedOn = DateTime.UtcNow;
                        contentManagement.UpdatedBy = merchantId;
                        contentManagement.Description = model.Description;
                        contentManagement.IsActive = model.IsActive;
                        contentManagement.Title = model.Title;

                        _context.ContentManagement.Update(contentManagement);
                        await _context.SaveChangesAsync();
                    }

                    await trans.CommitAsync();

                    return (true, MessageConstant.UpdateTermConditionSuccess);
                }
                catch
                {
                    await trans.RollbackAsync();
                    throw;
                }
            }
        }

        #endregion
    }
}
