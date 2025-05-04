using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.ApiViewModels.Customer.Request;
using HealthLayby.Models.ApiViewModels.Customer.Response;
using HealthLayby.Models.ApiViewModels.Registration.Request;
using HealthLayby.Models.ApiViewModels.Registration.Response;
using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using ApiCustomerModel = HealthLayby.Models.ApiViewModels.Common.CustomerModel;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// customer service
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.ICustomerRepository" />
    /// <seealso cref="Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.ICategoryRepository" />
    public class CustomerService : BaseService, ICustomerRepository
    {
        #region Private Variable 

        /// <summary>
        /// The stripe application service
        /// </summary>
        private readonly IStripeAppService _stripeAppService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="stripeAppService">The stripe application service.</param>
        public CustomerService(AppDbContext context, IStripeAppService stripeAppService) : base(context)
        {
            _stripeAppService = stripeAppService;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Customers the grid list asynchronous.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public async Task<Tuple<List<CustomerGridListResult>, int, int>> CustomerGridListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText)
        {
            try
            {
                var paramTotalRecords = new OutputParameter<int?>();
                var paramTotalFilteredRecords = new OutputParameter<int?>();

                var result = await _context.GetProcedures().CustomerGridListAsync
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
        /// Gets the customer model by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<CustomerModel?> GetCustomerModelByIdAsync(long id)
        {
            try
            {
                var customerModel = await _context.Customer.Where(q => q.CustomerId == id
                                                       && !q.IsDeleted
                                                    )
                                              .Select(q => new CustomerModel
                                              {
                                                  CustomerId = q.CustomerId,
                                                  Email = q.EmailAddress,
                                                  FirstName = q.FirstName,
                                                  IsActive = q.IsActive,
                                                  LastName = q.LastName,
                                                  PhoneNumber = q.PhoneNumber,
                                                  RegisteredOn = q.CreatedOn.ToString("MMM d, yyyy"),
                                                  Plans = q.TotalPlans,
                                                  WalletAmt = q.WalletAmount,
                                                  FamilyMemberFirstName = q.CustomerFamilyMember.FirstOrDefault().FirstName,
                                                  FamilyMemberLastName = q.CustomerFamilyMember.FirstOrDefault().LastName,
                                                  FamilyRelation = q.CustomerFamilyMember.FirstOrDefault().Relation,
                                                  ProfilePic =q.ProfilePic
                                              }).FirstOrDefaultAsync();

                customerModel.customerEmergencyContacts = await _context.CustomerEmergencyContact.Where(x => x.CustomerId == id && !x.IsDeleted).ToListAsync();

                return customerModel;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Saves the customer asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveCustomerAsync(CustomerModel model, long adminId, string password)
        {
            var message = string.Empty;
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                if (await IsCustomerEmailAvailableAsync(model.CustomerId, model.Email))
                {
                    return (false, MessageConstant.EmailAlredyExist);
                }

                if (await IsCustomerPhoneNumberAvailableAsync(model.CustomerId, model.PhoneNumber))
                {
                    return (false, MessageConstant.PhoneNumberAlreadyExist);
                }


                var customer = await _context.Customer.FirstOrDefaultAsync(q => q.CustomerId == model.CustomerId && !q.IsDeleted);

                if (customer is not null)
                {
                    customer.FirstName = model.FirstName;
                    customer.LastName = model.LastName;
                    customer.EmailAddress = model.Email;
                    customer.PhoneNumber = model.PhoneNumber;
                    customer.UpdatedOn = DateTime.UtcNow;
                    customer.UpdatedBy = adminId;
                    customer.IsActive = model.IsActive;

                    _context.Customer.Update(customer);
                    await _context.SaveChangesAsync();

                    await _stripeAppService.UpdateStripeCustomerAsync
                    (
                        stripeCustomerId: customer.StripeId,
                        customer: new Stripe.CustomerUpdateOptions
                        {
                            Email = model.Email,
                            Name = $"{model.FirstName} {model.LastName}"
                        }
                    );
                    message = MessageConstant.UpdateCustomerSuccess;
                }
                else
                {
                    var stripeCustomer = await _stripeAppService.AddStripeCustomerAsync(new Stripe.CustomerCreateOptions
                    {
                        Email = model.Email,
                        Name = $"{model.FirstName} {model.LastName}"
                    });

                    customer = new Customer
                    {
                        EmailAddress = model.Email,
                        FirstName = model.FirstName,
                        IsActive = model.IsActive,
                        LastName = model.LastName,
                        PhoneNumber = model.PhoneNumber,
                        Password = password,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = adminId,
                        StripeId = stripeCustomer.CustomerId
                    };

                    await _context.Customer.AddAsync(customer);
                    await _context.SaveChangesAsync();
                    message = MessageConstant.CreateCustomerSuccess;

                }
                if (model.CustomerId == 0)
                {
                    CustomerEmergencyContact customerEmergencyContact = new CustomerEmergencyContact();
                    customerEmergencyContact.CustomerId = customer.CustomerId;
                    customerEmergencyContact.FirstName = model.EmergencyFirstName;
                    customerEmergencyContact.LastName = model.EmergencyLastName;
                    customerEmergencyContact.Email = model.EmergencyEmail;
                    customerEmergencyContact.Number = model.EmergencyMobileNumber;
                    customerEmergencyContact.CreatedOn = DateTime.UtcNow;
                    await _context.CustomerEmergencyContact.AddAsync(customerEmergencyContact);
                    await _context.SaveChangesAsync();
                }
                if (model.customerEmergencyContacts != null && model.customerEmergencyContacts.Count() > 0)
                {
                    foreach (var EmergencyContact in model.customerEmergencyContacts)
                    {
                        if (EmergencyContact.EmergencyContactId > 0)
                        {
                            var customerEmergencyContact = await _context.CustomerEmergencyContact.FirstOrDefaultAsync(q => q.EmergencyContactId == EmergencyContact.EmergencyContactId && !q.IsDeleted);
                            if (customerEmergencyContact != null)
                            {
                                customerEmergencyContact.CustomerId = customer.CustomerId;
                                customerEmergencyContact.FirstName = EmergencyContact.FirstName;
                                customerEmergencyContact.LastName = EmergencyContact.LastName;
                                customerEmergencyContact.Email = EmergencyContact.Email;
                                customerEmergencyContact.Number = EmergencyContact.Number;
                                customerEmergencyContact.UpdatedOn = DateTime.UtcNow;
                                _context.CustomerEmergencyContact.Update(customerEmergencyContact);
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(EmergencyContact.FirstName) && !string.IsNullOrWhiteSpace(EmergencyContact.Email))
                            {
                                EmergencyContact.CustomerId = customer.CustomerId;
                                EmergencyContact.CreatedOn = DateTime.UtcNow;
                                await _context.CustomerEmergencyContact.AddAsync(EmergencyContact);
                                await _context.SaveChangesAsync();
                            }
                        }

                    }
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
        /// Deletes the customer asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> DeleteCustomerAsync(long id, long adminId)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var customer = await _context.Customer.FirstOrDefaultAsync(q => q.CustomerId == id && !q.IsDeleted);

                if (customer is null)
                {
                    return Tuple.Create(false, MessageConstant.CustomerNotFound);
                }

                customer.IsDeleted = true;
                customer.DeletedOn = DateTime.UtcNow;
                customer.DeletedBy = adminId;

                _context.Customer.Update(customer);
                await _context.SaveChangesAsync();

                await trans.CommitAsync();
                return Tuple.Create(true, MessageConstant.DeleteCustomerSuccess);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is customer email available] [the specified email] asynchronous.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public async Task<bool> IsCustomerEmailAvailableAsync(long customerId, string email)
        {
            try
            {
                return await _context.Customer.AnyAsync(x => x.CustomerId != customerId
                                                          && x.EmailAddress == email
                                                          && !x.IsDeleted
                                                       );
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Customers the grid top record list asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TopCustomerGridListResult>> TopCustomerGridListAsync()
        {
            try
            {
                return await _context.GetProcedures().TopCustomerGridListAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the customer count asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetCustomerCountAsync()
        {
            try
            {
                return await _context.Customer.CountAsync(x => !x.IsDeleted);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the customer chart data asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, int>> GetCustomerChartDataAsync()
        {
            try
            {
                var result = new Dictionary<string, int>();

                var currentDateTime = DateTime.Now;
                var fromDate = currentDateTime.AddMonths(-6);
                var customerCreatedDates = await _context.Customer.Where(q => !q.IsDeleted
                                                                           && q.CreatedOn.Date >= fromDate.Date
                                                                        )
                                                                  .Select(q => q.CreatedOn)
                                                                  .ToListAsync();



                for (var count = 0; count <= 5; count++)
                {
                    var monthDate = currentDateTime.AddMonths(-count);

                    var monthCount = customerCreatedDates.Count(x => x.Month == monthDate.Month
                                                                  && x.Year == monthDate.Year
                                                               );

                    result.Add
                    (
                        $"{monthDate.ToString("MMM")}-{monthDate.Year}",
                        monthCount
                    );
                }

                return result.Reverse().ToDictionary(x => x.Key, x => x.Value);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the customer by email address asynchronous.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        public async Task<ApiCustomerModel?> GetCustomerByEmailAddressAsync(string emailAddress)
        {
            try
            {
                return await _context.Customer.Where(x => x.EmailAddress == emailAddress
                                                       && x.IsActive
                                                       && !x.IsDeleted
                                                    )
                                              .Select(q => new ApiCustomerModel
                                              {
                                                  CustomerId = q.CustomerId,
                                                  FirstName = q.FirstName,
                                                  LastName = q.LastName,
                                                  EmailAddress = q.EmailAddress,
                                                  PhoneNumber = q.PhoneNumber,
                                                  Password = q.Password,
                                                  FullName = q.FullName,
                                                  ProfilePic = q.ProfilePic
                                              }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the customer by identifier asynchronous.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        public async Task<Customer?> GetCustomerByCustomerIdAsync(long customerId)
        {
            try
            {
                return await _context.Customer.FirstOrDefaultAsync(x => x.CustomerId == customerId);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the customer password asynchronous.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<bool> UpdateCustomerPasswordAsync(long customerId, string password)
        {
            try
            {
                var customer = await _context.Customer.FirstOrDefaultAsync(x => x.CustomerId == customerId);

                if (customer is null)
                {
                    return false;
                }

                customer.Password = password;

                _context.Customer.Update(customer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the profile details.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        public async Task<ProfileDetailsResponse?> GetProfileDetails(long customerId)
        {
            try
            {
                return await _context.Customer.Where(x => x.CustomerId == customerId
                                                       && !x.IsDeleted
                                                    )
                                              .Select(x => new ProfileDetailsResponse
                                              {
                                                  ProfilePicPath = x.ProfilePic,
                                                  CustomerId = x.CustomerId,
                                                  FirstName = x.FirstName,
                                                  LastName = x.LastName,
                                                  EmailAddress = x.EmailAddress,
                                                  PhoneNumber = x.PhoneNumber,
                                                  FullName = x.FullName,
                                                  ProfilePic = x.ProfilePic,
                                                  BirthDate = x.BirthDate,
                                                  CustomerAddress = x.CustomerAddress,
                                                  //EmergencyContactFirstName = x.EmergencyContactFirstName,
                                                  //EmergencyContactLastName = x.EmergencyContactLastName,
                                                  //EmergencyContactNumber = x.EmergencyContactNumber,
                                                  //EmergencyContactEmail = x.EmergencyContactEmail,
                                                  FamilyMembers = x.CustomerFamilyMember.Where(y => !y.IsDeleted).Select(fm => new FamilyMemberResponse
                                                  {
                                                      FamilyMemberId = fm.FamilyMemberId,
                                                      CustomerId = fm.CustomerId,
                                                      FirstName = fm.FirstName,
                                                      LastName = fm.LastName,
                                                      Relation = fm.Relation
                                                  }).ToList(),
                                                  //EmergencyContacts = x.CustomerEmergencyContact.Where(z => !z.IsDeleted).Select(ec => new EmergencyContactResponse
                                                  //{
                                                  //    EmergencyContactId = ec.EmergencyContactId,
                                                  //    CustomerId = ec.CustomerId,
                                                  //    FirstName = ec.FirstName,
                                                  //    LastName = ec.LastName,
                                                  //    Number = ec.Number,
                                                  //    Email = ec.Email
                                                  //}).ToList(),
                                              }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the profile details.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="updateProfileDetailsRequest">The update profile details request.</param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateProfileDetails(long customerId, UpdateProfileDetailsRequest updateProfileDetailsRequest)
        {
            try
            {
                var customer = await _context.Customer
                                             .FirstOrDefaultAsync(x =>
                                                                        x.CustomerId == customerId
                                                                        && x.IsActive
                                                                        && !x.IsDeleted
                                                                  );

                if (customer is null)
                {
                    return (false, ApiMessages.ProfileNotUpdated);
                }

                customer.FirstName = updateProfileDetailsRequest.FirstName;
                customer.LastName = updateProfileDetailsRequest.LastName;
                customer.EmailAddress = updateProfileDetailsRequest.EmailAddress;
                customer.PhoneNumber = updateProfileDetailsRequest.PhoneNumber;
                customer.ProfilePic = updateProfileDetailsRequest.ProfilePic ?? customer.ProfilePic;
                customer.BirthDate = updateProfileDetailsRequest.BirthDate ?? customer.BirthDate;
                customer.CustomerAddress = updateProfileDetailsRequest.CustomerAddress ?? customer.CustomerAddress;

                customer.UpdatedBy = customerId;
                customer.UpdatedOn = DateTime.UtcNow;

                _context.Customer.Update(customer);

                await _context.SaveChangesAsync();
                return (true, ApiMessages.ProfileUpdatedSuccessfully);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Inserts the update customer family member.
        /// </summary>
        /// <param name="saveFamilyMemberRequest">The save family member request.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveFamilyMember(SaveFamilyMemberRequest saveFamilyMemberRequest, long customerId)
        {
            try
            {
                var customerFamilyMember = await _context.CustomerFamilyMember.FirstOrDefaultAsync(x => x.FamilyMemberId == saveFamilyMemberRequest.FamilyMemberId);

                if (customerFamilyMember is null)
                {
                    customerFamilyMember = new CustomerFamilyMember
                    {
                        CustomerId = customerId,
                        CreatedOn = DateTime.UtcNow
                    };
                }
                else
                {
                    customerFamilyMember.UpdatedOn = DateTime.UtcNow;
                }

                customerFamilyMember.FirstName = saveFamilyMemberRequest.FirstName;
                customerFamilyMember.LastName = saveFamilyMemberRequest.LastName;
                customerFamilyMember.Relation = saveFamilyMemberRequest.Relation;

                if (customerFamilyMember.FamilyMemberId > 0)
                {
                    _context.CustomerFamilyMember.Update(customerFamilyMember);
                }
                else
                {
                    _context.CustomerFamilyMember.Add(customerFamilyMember);
                }

                await _context.SaveChangesAsync();
                return (true, ApiMessages.FamilyMemberSavedSuccessfully);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the customer family member.
        /// </summary>
        /// <param name="familyMemberId">The family member identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteFamilyMember(long familyMemberId)
        {
            try
            {
                var customerFamilyMember = await _context.CustomerFamilyMember
                                                         .FirstOrDefaultAsync(x =>
                                                                                x.FamilyMemberId == familyMemberId
                                                                                && !x.IsDeleted
                                                                            );

                if (customerFamilyMember is null)
                {
                    return (false, ApiMessages.FamilyMemberNotDeleted);
                }

                customerFamilyMember.IsDeleted = true;
                customerFamilyMember.DeletedOn = DateTime.UtcNow;

                _context.CustomerFamilyMember.Update(customerFamilyMember);

                await _context.SaveChangesAsync();
                return (true, ApiMessages.FamilyMemberDeletedSuccessfully);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the emergency contacts.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        public async Task<List<EmergencyContactResponse>?> GetEmergencyContacts(long customerId)
        {
            try
            {
                return await _context.CustomerEmergencyContact.Where(x => x.CustomerId == customerId
                                                                       && !x.IsDeleted
                                                                    )
                                                              .Select(ec => new EmergencyContactResponse
                                                              {
                                                                  EmergencyContactId = ec.EmergencyContactId,
                                                                  CustomerId = ec.CustomerId,
                                                                  FirstName = ec.FirstName,
                                                                  LastName = ec.LastName,
                                                                  Number = ec.Number,
                                                                  Email = ec.Email
                                                              }).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Saves the emergency contact.
        /// </summary>
        /// <param name="saveEmergencyContactRequest">The save emergency contact request.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveEmergencyContact(SaveEmergencyContactRequest saveEmergencyContactRequest, long customerId)
        {
            try
            {
                var customerEmergencyContact = await _context.CustomerEmergencyContact.FirstOrDefaultAsync(x => x.EmergencyContactId == saveEmergencyContactRequest.EmergencyContactId);

                if (customerEmergencyContact is null)
                {
                    customerEmergencyContact = new CustomerEmergencyContact
                    {
                        CustomerId = customerId,
                        CreatedOn = DateTime.UtcNow
                    };
                }
                else
                {
                    customerEmergencyContact.UpdatedOn = DateTime.UtcNow;
                }

                customerEmergencyContact.FirstName = saveEmergencyContactRequest.FirstName;
                customerEmergencyContact.LastName = saveEmergencyContactRequest.LastName;
                customerEmergencyContact.Number = saveEmergencyContactRequest.Number;
                customerEmergencyContact.Email = saveEmergencyContactRequest.Email;

                if (customerEmergencyContact.EmergencyContactId > 0)
                {
                    _context.CustomerEmergencyContact.Update(customerEmergencyContact);
                }
                else
                {
                    _context.CustomerEmergencyContact.Add(customerEmergencyContact);
                }

                await _context.SaveChangesAsync();

                return (true, ApiMessages.EmergencyContactSavedSuccessfully);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the customer emergency contact.
        /// </summary>
        /// <param name="emergencyContactId">The emergency contact identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteEmergencyContact(long emergencyContactId)
        {
            try
            {
                var customerEmergencyContact = await _context.CustomerEmergencyContact
                                                             .FirstOrDefaultAsync(x =>
                                                                                    x.EmergencyContactId == emergencyContactId
                                                                                    && !x.IsDeleted
                                                                                  );

                if (customerEmergencyContact is null)
                {
                    return (false, ApiMessages.EmergencyContactNotDeleted);
                }

                customerEmergencyContact.IsDeleted = true;
                customerEmergencyContact.DeletedOn = DateTime.UtcNow;

                _context.CustomerEmergencyContact.Update(customerEmergencyContact);

                await _context.SaveChangesAsync();
                return (true, ApiMessages.EmergencyContactDeletedSuccessfully);
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Mobile API's

        /// <summary>
        ///   Saves the customer asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<(bool, string, SignUpResponse)> SaveCustomerAsyncForAPI(SignUpRequest model)
        {
            var message = string.Empty;
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var stripeCustomer = await _stripeAppService.AddStripeCustomerAsync(new Stripe.CustomerCreateOptions
                {
                    Email = model.EmailAddress,
                    Name = $"{model.FirstName} {model.LastName}"
                });

                Customer customer = new Customer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    Password = CommonLogic.Encrypt(model.Password),
                    FullName = model.FirstName + model.LastName,
                    IsActive = true,
                    CreatedOn = DateTime.UtcNow,
                    StripeId = stripeCustomer.CustomerId
                };

                await _context.Customer.AddAsync(customer);
                await _context.SaveChangesAsync();
                message = ApiMessages.SuccessRegistration;

                await trans.CommitAsync();

                SignUpResponse signUpResponse = new SignUpResponse()
                {
                    CustomerId = customer.CustomerId,
                    FullName = customer.FullName,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Password = CommonLogic.Decrypt(customer.Password),
                    EmailAddress = customer.EmailAddress,
                    StripID = customer.StripeId
                };

                if (signUpResponse.CustomerId > 0)
                    return (true, message, signUpResponse);
                else
                    return (false, ApiMessages.InvalidRegistration, null);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is customer email available asynchronous for API] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public async Task<bool> IsCustomerEmailAvailableAsyncForAPI(string email)
        {
            try
            {
                return await _context.Customer.AnyAsync(x => x.EmailAddress == email
                                                          && !x.IsDeleted
                                                       );
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> IsCustomerEmergencyEmailAvailableAsync(long emergencyContactId, string email)
        {
            try
            {
                return await _context.CustomerEmergencyContact.AnyAsync(x => x.EmergencyContactId != emergencyContactId
                                                          && x.Email == email
                                                          && !x.IsDeleted
                                                       );
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Determines whether [is customer phone number available asynchronous] [the specified customer identifier].
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns></returns>
        public async Task<bool> IsCustomerPhoneNumberAvailableAsync(long customerId, string phoneNumber)
        {
            try
            {
                return await _context.Customer.AnyAsync(x => x.CustomerId != customerId
                                                          && x.PhoneNumber == phoneNumber
                                                          && !x.IsDeleted
                                                       );
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the customer account.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteCustomerAccount(long customerId)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var customer = await _context.Customer
                                             .FirstOrDefaultAsync(x =>
                                                                        x.CustomerId == customerId
                                                                        && x.IsActive
                                                                        && !x.IsDeleted
                                                                  );

                if (customer is null)
                {
                    return (false, ApiMessages.ProfileNotDeleted);
                }

                customer.IsActive = false;
                customer.IsDeleted = true;
                customer.DeletedBy = customerId;
                customer.DeletedOn = DateTime.UtcNow;

                _context.Customer.Update(customer);

                await _context.SaveChangesAsync();
                await trans.CommitAsync();

                return (true, ApiMessages.ProfileDeletedSuccessfully);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }


        #endregion
    }
}