using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// Reward Controller
    /// </summary>
    /// <seealso cref="HealthLayby.Admin.Controllers.BaseController" />
    [Authorize]
    public class RewardsController : BaseController
    {
        #region Private Variables

        /// <summary>
        /// The rewards repository
        /// </summary>
        private readonly IRewardsRepository _rewardsRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RewardsController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="rewardsRepository">The rewards repository.</param>
        public RewardsController(IHttpContextAccessor httpContextAccessor, 
                                    IRewardsRepository rewardsRepository) : base(httpContextAccessor)
        {
            _rewardsRepository = rewardsRepository;
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
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(long id = 0)
        {
            try
            {
                return PartialView("_Edit", await _rewardsRepository.GetRewardModelByIdAsync(id) ?? new RewardModel());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the reward list.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetRewardList()
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
                    "0" => "ID",
                    "1" => "Month",
                    "2" => "Discount",
                    "3" => "CategoryName",
                    "4" => "MerchantName",
                    "5" => "IsActive",
                    _ => "CreatedOn",
                };

                var (data, totalRecord, totalFilteredRecord) = await _rewardsRepository.RewardGridListAsync
                (
                    sortColumn: sortingColumnName,
                    sortOrder: string.IsNullOrWhiteSpace(orderDirection.ToString()) ? "desc" : orderDirection.ToString(),
                    pageSize: Convert.ToInt32(pageSize),
                    pageIndex: Convert.ToInt32(skipRecord),
                    searchText: searchText
                );

                return Json(new
                {
                    draw = Convert.ToInt32(draw),
                    customerCount = totalRecord,
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
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(RewardModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = MessageConstant.InvalidModalState });
                }

                var (isSuccess, message) = await _rewardsRepository.SaveRewardAsync(model, claim.AdminId);

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
                var (isSuccess, message) = await _rewardsRepository.DeleteRewardAsync(id, claim.AdminId);

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

        #endregion

    }
}
