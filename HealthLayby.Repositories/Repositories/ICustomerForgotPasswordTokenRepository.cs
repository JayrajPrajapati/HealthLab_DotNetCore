using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// ICustomerForgotPasswordToken
    /// </summary>
    public interface ICustomerForgotPasswordTokenRepository
    {
        /// <summary>
        /// Adds the forgot password token for customer asynchronous.
        /// </summary>
        /// <param name="CustomerId">The customer identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> AddForgotPasswordTokenForCustomerAsync(long CustomerId, Guid token);
        /// <summary>
        /// Gets the unused token for customer asynchronous.
        /// </summary>
        /// <param name="CustomerId">The customer identifier.</param>
        /// <returns></returns>
        Task<Guid?> GetUnusedTokenForCustomerAsync(long CustomerId);
        /// <summary>
        /// Checks the customer token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> CheckCustomerTokenAsync(Guid token);
        /// <summary>
        /// Gets the customer identifier by token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<long?> GetCustomerIdByTokenAsync(Guid token);
        /// <summary>
        /// Updates the change date asynchronous.
        /// </summary>
        /// <param name="Token">The token.</param>
        /// <returns></returns>
        Task<bool> UpdateChangeDateAsync(Guid Token);
    }
}
