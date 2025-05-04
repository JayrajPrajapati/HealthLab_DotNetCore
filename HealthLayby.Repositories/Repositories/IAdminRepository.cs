using HealthLayby.Models.AdminViewModels;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// admin repository
    /// </summary>
    public interface IAdminRepository
    {
        /// <summary>
        /// Gets the admin by email address asynchronous.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        Task<AdminModel?> GetAdminByEmailAddressAsync(string emailAddress);

        /// <summary>
        /// Gets the admin by admin identifier asynchronous.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<AdminModel?> GetAdminByAdminIdAsync(long adminId);

        /// <summary>
        /// Updates the admin password asynchronous.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<(bool, string)> UpdateAdminPasswordAsync(long adminId, ChangePasswordModel model);

        /// <summary>
        /// Updates the admin profile asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns></returns>
        Task<(bool, string)> UpdateAdminProfileAsync(long id, string firstName, string lastName);

        /// <summary>
        /// Updates the admin reset password asynchronous.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<bool> UpdateAdminResetPasswordAsync(long adminId, string password);
    }
}
