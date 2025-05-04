using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// forgotpassword service
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.IForgotPasswordRepository" />
    public class ForgotPasswordService : BaseService, IForgotPasswordRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public ForgotPasswordService(AppDbContext context) : base(context)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the forgot password token for admin asynchronous.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<bool> AddForgotPasswordTokenForAdminAsync(long adminId, Guid token)
        {
            try
            {
                var model = new ForgotPasswordToken
                {
                    AdminId = adminId,
                    Token = token,
                    CreatedOn = DateTime.UtcNow
                };
                await _context.ForgotPasswordToken.AddAsync(model);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the unused token for admin asynchronous.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<Guid?> GetUnusedTokenForAdminAsync(long adminId)
        {
            try
            {
                return await _context.ForgotPasswordToken.Where(x => x.AdminId.HasValue
                                                                  && x.AdminId.Value == adminId
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
        /// Checks the admin token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<bool> CheckAdminTokenAsync(Guid token)
        {
            try
            {
                return await _context.ForgotPasswordToken.AnyAsync(q => q.Token == token && q.AdminId.HasValue && !q.ChangedOn.HasValue);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the admin identifier by token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public async Task<long?> GetAdminIdByTokenAsync(Guid token)
        {
            try
            {
                return await _context.ForgotPasswordToken.Where(x => x.Token == token
                                                                  && x.AdminId.HasValue
                                                               )
                                                         .Select(x => x.AdminId)
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
                var forgotPasswordTokenDate = await _context.ForgotPasswordToken.Where(x => x.Token == Token)
                                           .FirstOrDefaultAsync();
                if (forgotPasswordTokenDate is not null)
                {
                    forgotPasswordTokenDate.ChangedOn = DateTime.UtcNow;
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
