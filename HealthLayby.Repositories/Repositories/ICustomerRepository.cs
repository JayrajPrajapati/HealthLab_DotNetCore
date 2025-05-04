using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.ApiViewModels.Customer.Request;
using HealthLayby.Models.ApiViewModels.Customer.Response;
using HealthLayby.Models.ApiViewModels.Registration.Request;
using HealthLayby.Models.ApiViewModels.Registration.Response;
using HealthLayby.Models.Models;
using ApiCustomerModel = HealthLayby.Models.ApiViewModels.Common.CustomerModel;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// customer repository
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Customers the grid list asynchronous.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        Task<Tuple<List<CustomerGridListResult>, int, int>> CustomerGridListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText);

        /// <summary>
        /// Gets the customer model by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<CustomerModel?> GetCustomerModelByIdAsync(long id);

        /// <summary>
        /// Saves the customer asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveCustomerAsync(CustomerModel model, long adminId, string password);

        /// <summary>
        /// Deletes the customer asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<Tuple<bool, string>> DeleteCustomerAsync(long id, long adminId);

        /// <summary>
        /// Determines whether [is customer email available] [the specified email] asynchronous.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Task<bool> IsCustomerEmailAvailableAsync(long customerId, string email);

        /// <summary>
        /// Customers the grid top record list asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<TopCustomerGridListResult>> TopCustomerGridListAsync();

        /// <summary>
        /// Gets the customer count asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<int> GetCustomerCountAsync();

        /// <summary>
        /// Gets the customer chart data asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<Dictionary<string, int>> GetCustomerChartDataAsync();

        /// <summary>
        /// Gets the customer by email address asynchronous.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        Task<ApiCustomerModel?> GetCustomerByEmailAddressAsync(string emailAddress);

        /// <summary>
        /// Gets the customer by identifier asynchronous.
        /// </summary>
        /// <param name="CustomerId">The customer identifier.</param>
        /// <returns></returns>
        Task<Customer?> GetCustomerByCustomerIdAsync(long CustomerId);

        /// <summary>
        /// Updates the customer password asynchronous.
        /// </summary>
        /// <param name="CustomerId">The customer identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<bool> UpdateCustomerPasswordAsync(long CustomerId, string password);

        /// <summary>
        /// Gets the profile details.
        /// </summary>
        /// <param name="CustomerId">The customer identifier.</param>
        /// <returns></returns>
        Task<ProfileDetailsResponse?> GetProfileDetails(long CustomerId);

        /// <summary>
        /// Updates the profile details.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="updateProfileDetailsRequest">The update profile details request.</param>
        /// <returns></returns>
        Task<(bool, string)> UpdateProfileDetails(long customerId, UpdateProfileDetailsRequest updateProfileDetailsRequest);

        /// <summary>
        /// Inserts the update customer family member.
        /// </summary>
        /// <param name="saveFamilyMemberRequest">The save family member request.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveFamilyMember(SaveFamilyMemberRequest saveFamilyMemberRequest, long customerId);

        /// <summary>
        /// Deletes the customer family member.
        /// </summary>
        /// <param name="familyMemberId">The family member identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> DeleteFamilyMember(long familyMemberId);

        /// <summary>
        /// Gets the emergency contacts.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        Task<List<EmergencyContactResponse>?> GetEmergencyContacts(long customerId);

        /// <summary>
        /// Saves the emergency contact.
        /// </summary>
        /// <param name="saveEmergencyContactRequest">The save emergency contact request.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveEmergencyContact(SaveEmergencyContactRequest saveEmergencyContactRequest, long customerId);

        /// <summary>
        /// Deletes the customer emergency contact.
        /// </summary>
        /// <param name="emergencyContactId">The emergency contact identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> DeleteEmergencyContact(long emergencyContactId);

        /// <summary>
        /// Saves the customer asynchronous for API.
        /// </summary>
        /// <param name="signUpRequest">The sign up request.</param>
        /// <returns></returns>
        Task<(bool, string, SignUpResponse)> SaveCustomerAsyncForAPI(SignUpRequest signUpRequest);

        /// <summary>
        /// Determines whether [is customer email available asynchronous for API] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Task<bool> IsCustomerEmailAvailableAsyncForAPI(string email);

        /// <summary>
        /// Determines whether [is customer email available] [the specified email] asynchronous.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Task<bool> IsCustomerEmergencyEmailAvailableAsync(long customerId, string email);



        /// <summary>
        /// Determines whether [is customer phone number available asynchronous] [the specified customer identifier].
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="phoneNumber">The phone number.</param>
        /// <returns></returns>
        Task<bool> IsCustomerPhoneNumberAvailableAsync(long customerId, string phoneNumber);

        /// <summary>
        /// Deletes the customer account.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> DeleteCustomerAccount(long customerId);
    }
}
