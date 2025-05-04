using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// CustomerForgotPasswordTokenService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.ICustomerForgotPasswordTokenRepository" />
    public class CustomerForgotPasswordTokenService : BaseService, ICustomerForgotPasswordTokenRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CustomerForgotPasswordTokenService(AppDbContext context) : base(context)
        {

        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Adds the forgot password token for customer asynchronous.
        /// </summary>
        /// <param name="CustomerId">The customer identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<bool> AddForgotPasswordTokenForCustomerAsync(long CustomerId, Guid token)
        {
            try
            {
                var model = new CustomerForgotPasswordToken
                {
                    CustomerId = CustomerId,
                    Token = token,
                    CreatedOn = DateTime.UtcNow
                };
                await _context.CustomerForgotPasswordToken.AddAsync(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the unused token for customer asynchronous.
        /// </summary>
        /// <param name="CustomerId">The customer identifier.</param>
        /// <returns></returns>
        public async Task<Guid?> GetUnusedTokenForCustomerAsync(long CustomerId)
        {
            try
            {
                return await _context.CustomerForgotPasswordToken.Where(x => x.CustomerId.HasValue
                                                                  && x.CustomerId.Value == CustomerId
                                                                  && !x.ChangedOn.HasValue
                                                               )
                                                         .Select(x => x.Token)
                                                         .FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Checks the customer token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<bool> CheckCustomerTokenAsync(Guid token)
        {
            try
            {
                return await _context.CustomerForgotPasswordToken.AnyAsync(q => q.Token == token && q.CustomerId.HasValue && !q.ChangedOn.HasValue);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Gets the customer identifier by token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<long?> GetCustomerIdByTokenAsync(Guid token)
        {
            try
            {
                return await _context.CustomerForgotPasswordToken.Where(x => x.Token == token
                                                                  && x.CustomerId.HasValue
                                                               )
                                                         .Select(x => x.CustomerId)
                                                         .FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the change date asynchronous.
        /// </summary>
        /// <param name="Token">The token.</param>
        /// <returns></returns>
        public async Task<bool> UpdateChangeDateAsync(Guid Token)
        {
            try
            {
                var customerForgotPasswordToken = await _context.CustomerForgotPasswordToken.Where(x => x.Token == Token)
                                           .FirstOrDefaultAsync();
                if (customerForgotPasswordToken is not null)
                {
                    customerForgotPasswordToken.ChangedOn = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                throw;
            }
            return false;
        }

        #endregion
    }
}
