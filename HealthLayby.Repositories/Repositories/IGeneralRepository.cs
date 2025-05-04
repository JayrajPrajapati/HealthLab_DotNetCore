using Microsoft.AspNetCore.Mvc.Rendering;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// IGeneral Repository
    /// </summary>
    public interface IGeneralRepository
    {
        /// <summary>
        /// Gets the active merchant dropdown list asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItem>> GetActiveMerchantDropdownListAsync();

        /// <summary>
        /// Gets the active category dropdown list asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItem>> GetActiveCategoryDropdownListAsync();

        /// <summary>
        /// Gets the active service dropdown list asynchronous.
        /// </summary>
        /// <param name="categoryid">The categoryid.</param>
        /// <returns></returns>
        Task<List<SelectListItem>> GetActiveServiceDropdownListAsync(long categoryid);


        /// <summary>
        /// Gets the bank dropdown list list asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItem>> GetBankDropdownListListAsync();
    }
}
