using HealthLayby.Models.AdminViewModels;
using HealthLayby.Repositories.Repositories;
using log4net.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// Lay By Controller
    /// </summary>
    /// <seealso cref="HealthLayby.Admin.Controllers.BaseController" />
    public class LayByController : BaseController
    {

        #region Private Variable
        /// <summary>
        /// The lay by repository
        /// </summary>
        private readonly ILayByRepository _layByRepository;
        #endregion

        #region Constructor 
        /// <summary>
        /// Initializes a new instance of the <see cref="LayByController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="layByRepository">The lay by repository.</param>
        public LayByController(IHttpContextAccessor httpContextAccessor, ILayByRepository layByRepository) : base(httpContextAccessor)
        {
            _layByRepository = layByRepository;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the customer plan lay by list.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> GetCustomerPlanLayByList()
        {
            try
            {
                Request.Form.TryGetValue("draw", out StringValues draw);
                Request.Form.TryGetValue("order[0][column]", out StringValues orderColumn);
                Request.Form.TryGetValue("order[0][dir]", out StringValues orderDirection);
                Request.Form.TryGetValue("start", out StringValues skipRecord);
                Request.Form.TryGetValue("length", out StringValues pageSize);
                Request.Form.TryGetValue("search[value]", out StringValues searchText);
                Request.Form.TryGetValue("status[value]", out StringValues status);



                long.TryParse(status, out long Status);
                string sortingColumnName = orderColumn.ToString() switch
                {
                    "1" => "MerchantName",
                    "2" => "CategoryName",
                    "3" => "ServiceName",
                    "4" => "Duration",
                    "5" => "Price",
                    "6" => "CustomerName",
                    _ => "MerchantName",
                };

                var (data, totalRecord, totalFilteredRecord) = await _layByRepository.GetLayByListAsync
                (
                    sortColumn: sortingColumnName,
                    sortOrder: string.IsNullOrWhiteSpace(orderDirection.ToString()) ? "desc" : orderDirection.ToString(),
                    pageSize: Convert.ToInt32(pageSize),
                    pageIndex: Convert.ToInt32(skipRecord),
                    searchText: searchText,
                    status: Status
                );

                return Json(new
                {
                    draw = Convert.ToInt32(draw),
                    merchantCount = totalRecord,
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
        /// Views the lay by.
        /// </summary>
        /// <param name="customerPlanId">The customer plan identifier.</param>
        /// <returns></returns>
        public async Task<IActionResult> ViewLayBy(long customerPlanId)
        {
            try
            {
                return View("_View", await _layByRepository.GetLayByByIdAsync(customerPlanId) ?? new LayByModel());
            }
            catch
            {

                throw;
            }
        }

        #endregion
    }
}
