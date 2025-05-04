using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Repositories.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace HealthLayby.Admin.Controllers
{
    /// <summary>
    /// customer controller
    /// </summary>
    /// <seealso cref="HealthLayby.Admin.Controllers.BaseController" />
    /// <seealso cref="BaseController" />
    [Authorize]
    public class CustomerController : BaseController
    {
        #region Private Variables

        /// <summary>
        /// The customer repository
        /// </summary>
        private readonly ICustomerRepository _customerRepository;

        /// <summary>
        /// The env
        /// </summary>
        private readonly IWebHostEnvironment _env;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="customerRepository">The customer repository.</param>
        /// <param name="env">The env.</param>
        public CustomerController(IHttpContextAccessor httpContextAccessor,
                                    ICustomerRepository customerRepository,
                                    IWebHostEnvironment env) : base(httpContextAccessor)
        {
            _customerRepository = customerRepository;
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
        /// Gets the customer list asynchronous.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetCustomerList()
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
                    "0" => "Customer",
                    "1" => "Phone",
                    "2" => "EmergencyNumber",
                    "3" => "CreatedOn",
                    "4" => "Plans",
                    "5" => "WalletAmt",
                    "6" => "Status",
                    _ => "RegisteredOn",
                };

                var (data, totalRecord, totalFilteredRecord) = await _customerRepository.CustomerGridListAsync
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
                    customerCount = totalRecord,
                    recordsTotal = totalRecord,
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
        /// Edits the specified identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(long id = 0)
        {
            try
            {
                return View("_Edit", await _customerRepository.GetCustomerModelByIdAsync(id) ?? new CustomerModel());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Creates the specified model asynchronous.
        /// </summary>
        /// <param name="customerModel">The customer model.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(CustomerModel customerModel)
        {
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    return Json(new { success = false, message = MessageConstant.InvalidModalState });
                //}

                var password = Password.Generate(10, 4);
                var (isSuccess, message) = await _customerRepository.SaveCustomerAsync(customerModel, claim.AdminId, CommonLogic.Encrypt(password));

                if (customerModel.CustomerId <= 0)
                {
                    SendEmailToCreatedCustomer
                    (
                        userName: $"{customerModel.FirstName} {customerModel.LastName}",
                        email: customerModel.Email,
                        password: password
                    );
                }

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
                var (isSuccess, message) = await _customerRepository.DeleteCustomerAsync(id, claim.AdminId);

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
        /// Determines whether [is customer email available] [the specified email].
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> IsCustomerEmailAvailable(long customerId, string email)
        {
            try
            {
                return Json(!await _customerRepository.IsCustomerEmailAvailableAsync(customerId, email));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Views the customer.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ViewCustomer(long id = 0)
        {
            try
            {
                return View("_View", await _customerRepository.GetCustomerModelByIdAsync(id) ?? new CustomerModel());
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
        public async Task<IActionResult> DeleteEmergencyContact(long id)
        {
            try
            {
                var (isSuccess, message) = await _customerRepository.DeleteEmergencyContact(id);

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
        /// Determines whether [is customer emegency email available asynchronous] [the specified emergency contact identifier].
        /// </summary>
        /// <param name="emergencyContactId">The emergency contact identifier.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> IsCustomerEmergencyEmailAvailableAsync(long emergencyContactId, string email)
        {
            try
            {
                return Json(!await _customerRepository.IsCustomerEmergencyEmailAvailableAsync(emergencyContactId, email));
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Private Methods 

        /// <summary>
        /// Gets the customer create HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        private string GetCustomerCreateHTML(string html, string userName, string email, string password)
        {
            try
            {
                var domain = $"{Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host.ToString()}";

                html = html.Replace("##FULLNAME##", userName);
                html = html.Replace("##EMAIL##", email);
                html = html.Replace("##PASSWORD##", password);
                html = html.Replace("##DOMAIN##", domain);
                return html;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Sends the email to created customer.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        private void SendEmailToCreatedCustomer(string userName, string email, string password)
        {
            var html = GetCustomerCreateHTML
            (
                html: System.IO.File.ReadAllText(Path.Combine(_env.WebRootPath, "templates/CustomerCreate.html")),
                userName: $"{userName}",
                email: email,
                password: password
            );

            CommonLogic.SendEmail
            (
               subject: "Your Health LayBy Account has been created!",
               body: html,
               toEmail: email
            );
        }

        #endregion
    }
}
