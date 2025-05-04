using HealthLayby.Models.Context;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Repositories.Services
{

    /// <summary>
    /// General Service
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.IGeneralRepository" />
    public class GeneralService : BaseService, IGeneralRepository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public GeneralService(AppDbContext context) : base(context)
        {
        }

        #region Public Methods

        #region Dropdown List

        /// <summary>
        /// Gets the active category dropdown list asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetActiveCategoryDropdownListAsync()
        {
            try
            {
                return await _context.Category.Where(q => !q.IsDeleted && q.IsActive)
                                              .Select(q => new SelectListItem
                                              {
                                                  Value = q.CategoryId.ToString(),
                                                  Text = q.CategoryName
                                              }).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the active merchant dropdown list asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetActiveMerchantDropdownListAsync()
        {
            try
            {
                return await _context.Merchant.Where(q => !q.IsDeleted && q.IsActive)
                                              .Select(q => new SelectListItem
                                              {
                                                  Text = q.FullName,
                                                  Value = q.MerchantId.ToString()
                                              }).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the active service dropdown list asynchronous.
        /// </summary>
        /// <param name="categoryid">The categoryid.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<List<SelectListItem>> GetActiveServiceDropdownListAsync(long categoryid)
        {
            try
            {
                return await _context.Service.Where(q => q.CategoryId == categoryid && !q.IsDeleted && q.IsActive)
                                              .Select(q => new SelectListItem
                                              {
                                                  Text = q.ServiceName,
                                                  Value = q.ServiceId.ToString()
                                              }).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the bank dropdown list list asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectListItem>> GetBankDropdownListListAsync()
        {
            try
            {
                return await _context.Bank.Select(q => new SelectListItem
                {
                    Text = q.BankName,
                    Value = q.BankId.ToString()
                }).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #endregion
    }
}
