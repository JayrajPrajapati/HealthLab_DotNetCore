namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// forgotpassword repository
    /// </summary>
    public interface IForgotPasswordRepository
    {
        /// <summary>
        /// Adds the forgot password token for admin asynchronous.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> AddForgotPasswordTokenForAdminAsync(long adminId, Guid token);

        /// <summary>
        /// Gets the unused token for admin asynchronous.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<Guid?> GetUnusedTokenForAdminAsync(long adminId);

        /// <summary>
        /// Checks the admin token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> CheckAdminTokenAsync(Guid token);

        /// <summary>
        /// Gets the admin identifier by token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<long?> GetAdminIdByTokenAsync(Guid token);

        /// <summary>
        /// Updates the change date asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> UpdateChangeDateAsync(Guid token);
    }
}
