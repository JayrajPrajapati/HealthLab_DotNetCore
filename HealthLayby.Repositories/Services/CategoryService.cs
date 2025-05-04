using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.ApiViewModels.Category;
using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// category service
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.ICategoryRepository" />
    public class CategoryService : BaseService, ICategoryRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CategoryService(AppDbContext context) : base(context)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves the category asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveCategoryAsync(CategoryModel model, long adminId)
        {
            string message = string.Empty;

            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                if (await IsCategoryNameAvailableAsync(model.CategoryId, model.CategoryName))
                {
                    return (false, message = MessageConstant.CategoryNameAlredyExist);
                }

                var category = await _context.Category.FirstOrDefaultAsync(x => x.CategoryId == model.CategoryId && !x.IsDeleted);

                if (category is null)
                {
                    var totalCount = await _context.Category.CountAsync();
                    var newCategoryCode = "HL" + (totalCount + 1).ToString().PadLeft(5, '0');

                    category = new Category
                    {
                        CategoryCode = newCategoryCode,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = adminId,
                        Image = model.ProfilePic,
                        CategoryName = model.CategoryName,
                        IsActive = model.IsActive
                    };

                    await _context.Category.AddAsync(category);
                    await _context.SaveChangesAsync();
                    message = MessageConstant.CreateCategorySuccess;
                }
                else
                {
                    category.UpdatedOn = DateTime.UtcNow;
                    category.UpdatedBy = adminId;
                    category.CategoryName = model.CategoryName;
                    category.Image = !string.IsNullOrWhiteSpace(model.ProfilePic) ? model.ProfilePic : string.Empty;
                    category.IsActive = model.IsActive;

                    _context.Category.Update(category);
                    await _context.SaveChangesAsync();

                    message = MessageConstant.UpdateCategorySuccess;
                }

                await trans.CommitAsync();
                return (true, message);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Gets the category model by identifier asynchronous.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        public async Task<CategoryModel?> GetCategoryModelByIdAsync(long categoryId)
        {
            try
            {
                return await _context.Category.Where(x => x.CategoryId == categoryId
                                                       && !x.IsDeleted)
                                              .Select(x => new CategoryModel
                                              {
                                                  CategoryId = x.CategoryId,
                                                  CategoryCode = x.CategoryCode,
                                                  CategoryName = x.CategoryName,
                                                  ProfilePic = x.Image,
                                                  IsActive = x.IsActive
                                              }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the category list.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public async Task<Tuple<List<CategoryGridListResult>, int, int>> GetCategoryListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText)
        {
            try
            {
                var paramTotalRecords = new OutputParameter<int?>();
                var paramTotalFilteredRecords = new OutputParameter<int?>();

                var result = await _context.GetProcedures().CategoryGridListAsync
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
        /// Deletes the category asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteCategoryAsync(long id, long adminId)
        {
            try
            {
                var isServiceAvailable = await _context.Service.AnyAsync(q => !q.IsDeleted && q.CategoryId == id);

                if (isServiceAvailable)
                {
                    return (false, MessageConstant.CategoryUsedInServiceFail);
                }

                var category = await _context.Category.FirstOrDefaultAsync(x => x.CategoryId == id && !x.IsDeleted);

                if (category is null)
                {
                    return (false, MessageConstant.CategoryNotFound);
                }

                category.IsDeleted = true;
                category.DeletedBy = adminId;
                category.DeletedOn = DateTime.UtcNow;

                _context.Category.Update(category);
                await _context.SaveChangesAsync();

                return (true, MessageConstant.DeleteCategorySuccess);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is category name available] [the specified category name].
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public async Task<bool> IsCategoryNameAvailableAsync(long categoryId, string categoryName)
        {
            try
            {
                return await _context.Category.AnyAsync(x => !x.IsDeleted && x.CategoryId != categoryId && x.CategoryName == categoryName);
            }
            catch
            {
                throw;
            }
        }

        #region API Methods

        /// <summary>
        /// Gets the category list asynchronous for API.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryListResponse>> GetCategoryListAsyncForAPI()
        {
            string message = string.Empty;

            try
            {
                return await _context.Category.Where(x => x.IsActive && !x.IsDeleted)
                                              .OrderBy(x => x.CategoryName)
                                              .Select(x => new CategoryListResponse
                                              {
                                                  CategoryId = x.CategoryId,
                                                  CategoryName = x.CategoryName,
                                                  CategoryCode = x.CategoryCode,
                                                  Image = x.Image == "" ? "" : x.Image
                                              })
                                              .ToListAsync();
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
