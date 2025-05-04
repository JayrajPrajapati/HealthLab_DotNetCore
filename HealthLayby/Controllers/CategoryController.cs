using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// category controller
    /// </summary>
    /// <seealso cref="HealthLayby.Admin.Controllers.BaseController" />
    [Authorize]
    public class CategoryController : BaseController
    {
        #region Private Variable 

        /// <summary>
        /// The category repository
        /// </summary>
        private readonly ICategoryRepository _categoryRepository;

        /// <summary>
        /// The env
        /// </summary>
        private readonly IWebHostEnvironment _env;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="categoryRepository">The category repository.</param>
        /// <param name="env">The env.</param>
        public CategoryController(IHttpContextAccessor httpContextAccessor,
                                    ICategoryRepository categoryRepository, IWebHostEnvironment env) : base(httpContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _env = env;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the category list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetCategoryList()
        {
            try
            {
                Request.Form.TryGetValue("draw", out StringValues draw);
                Request.Form.TryGetValue("order[0][column]", out StringValues orderColumn);
                Request.Form.TryGetValue("order[0][dir]", out StringValues orderDirection);
                Request.Form.TryGetValue("start", out StringValues skipRecord);
                Request.Form.TryGetValue("length", out StringValues pageSize);
                Request.Form.TryGetValue("search[value]", out StringValues searchText);

                string sortingColumnName = orderColumn.ToString() switch
                {
                    "0" => "CategoryCode",
                    "1" => "Category",
                    "2" => "Status",
                    "3" => "CreatedOn",
                    _ => "CreatedOn",
                };

                var (data, count, totalFilteredRecord) = await _categoryRepository.GetCategoryListAsync
                (
                    sortColumn: sortingColumnName,
                    sortOrder: orderDirection.ToString(),
                    pageSize: Convert.ToInt32(pageSize),
                    pageIndex: Convert.ToInt32(skipRecord),
                    searchText: searchText
                );

                return Json(new
                {
                    draw = Convert.ToInt32(draw),
                    categoryCount = count,
                    recordsTotal = totalFilteredRecord,
                    recordsFiltered = totalFilteredRecord,
                    data
                });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Edits the category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> EditCategory(long id = 0)
        {
            try
            {
                return View("_AddEdit", await _categoryRepository.GetCategoryModelByIdAsync(id) ?? new CategoryModel());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveCategory(CategoryModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = MessageConstant.InvalidModalState });
                }
                #region File Upload

                if (model.CategoryId > 0)
                {
                    var category = await _categoryRepository.GetCategoryModelByIdAsync(model.CategoryId);

                    if (category is not null)
                    {
                        var allowDeleteProfilePic = !string.IsNullOrWhiteSpace(model.ImageBase64)
                                                 && !string.IsNullOrWhiteSpace(model.ImageFileExtension);

                        if (allowDeleteProfilePic && !string.IsNullOrWhiteSpace(category.ProfilePic))
                        {
                            FileUploadHelper.DeleteFile
                            (
                                path: Path.Combine(_env.WebRootPath, DirectoryConstant.CategoryPicDirectory, category.ProfilePic)
                            );
                        }

                    }
                }

                if (model.ImageBase64 is not null && model.ImageFileExtension is not null)
                {
                    model.ProfilePic = FileUploadHelper.UploadFile
                    (
                        base64: model.ImageBase64,
                        extension: model.ImageFileExtension,
                        path: Path.Combine(_env.WebRootPath, DirectoryConstant.CategoryPicDirectory)
                    );
                }

                #endregion
                ViewBag.IsFromView = true;

                var (isSuccess, message) = await _categoryRepository.SaveCategoryAsync(model, claim.AdminId);

                return Json(new { success = isSuccess, message });
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var (isSuccess, message) = await _categoryRepository.DeleteCategoryAsync(id, claim.AdminId);

                return Json(new
                {
                    success = isSuccess,
                    message
                });
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// Determines whether [is category name available] [the specified category identifier].
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="categoryName">Name of the category.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        [HttpPost]
        public async Task<JsonResult> IsCategoryNameAvailable(long categoryId, string categoryName)
        {
            try
            {
                return Json(!await _categoryRepository.IsCategoryNameAvailableAsync(categoryId, categoryName));
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
