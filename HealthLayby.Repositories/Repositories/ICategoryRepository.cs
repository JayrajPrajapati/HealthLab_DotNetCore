using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.ApiViewModels.Category;
using HealthLayby.Models.Models;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// categoryrepository
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Saves the category asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveCategoryAsync(CategoryModel model, long adminId);

        /// <summary>
        /// Gets the category model by identifier asynchronous.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        Task<CategoryModel?> GetCategoryModelByIdAsync(long categoryId);

        /// <summary>
        /// Gets the category list.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        Task<Tuple<List<CategoryGridListResult>, int, int>> GetCategoryListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText);

        /// <summary>
        /// Deletes the category asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> DeleteCategoryAsync(long id, long adminId);

        /// <summary>
        /// Determines whether [is category name available] [the specified category name].
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns></returns>
        Task<bool> IsCategoryNameAvailableAsync(long categoryId, string categoryName);

        /// <summary>
        /// Gets the category list asynchronous for API.
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryListResponse>> GetCategoryListAsyncForAPI();
    }
}
