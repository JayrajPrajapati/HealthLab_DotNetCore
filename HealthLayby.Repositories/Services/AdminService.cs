using HealthLayby.Helpers.CommonMethod;
using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.Context;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// admin service
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.IAdminRepository" />
    public class AdminService : BaseService, IAdminRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public AdminService(AppDbContext context) : base(context)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the admin by email address asynchronous.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        public async Task<AdminModel?> GetAdminByEmailAddressAsync(string emailAddress)
        {
            try
            {
                return await _context.Admin.Where(x => x.EmailAddress == emailAddress)
                                           .Select(q => new AdminModel
                                           {
                                               AdminId = q.AdminId,
                                               EmailAddress = q.EmailAddress,
                                               FirstName = q.FirstName,
                                               FullName = q.FullName,
                                               LastName = q.LastName,
                                               Password = q.Password
                                           }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the admin by admin identifier asynchronous.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<AdminModel?> GetAdminByAdminIdAsync(long adminId)
        {
            try
            {
                return await _context.Admin.Where(x => x.AdminId == adminId)
                                           .Select(q => new AdminModel
                                           {
                                               AdminId = q.AdminId,
                                               EmailAddress = q.EmailAddress,
                                               FirstName = q.FirstName,
                                               FullName = q.FullName,
                                               LastName = q.LastName,
                                               Password = q.Password
                                           }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the admin password asynchronous.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateAdminPasswordAsync(long adminId, ChangePasswordModel model)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var admin = await _context.Admin.FirstOrDefaultAsync(x => x.AdminId == adminId && !x.IsDeleted);
                if (admin is null)
                {
                    return (false, MessageConstant.AdminNotFound);
                }

                if (CommonLogic.Decrypt(admin.Password) != model.Password)
                {
                    return (false, MessageConstant.AdminOldPasswordError);
                }

                admin.Password = CommonLogic.Encrypt(model.NewPassword);

                _context.Admin.Update(admin);
                await _context.SaveChangesAsync();

                await trans.CommitAsync();
                return (true, MessageConstant.AdminNewPasswordSuccess);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Updates the admin profile asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateAdminProfileAsync(long id, string firstName, string lastName)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var admin = await _context.Admin.FirstOrDefaultAsync(x => x.AdminId == id);
                if (admin is null)
                {
                    return (false, MessageConstant.AdminNotFound);
                }

                admin.FirstName = firstName;
                admin.LastName = lastName;

                _context.Admin.Update(admin);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return (true, MessageConstant.UpdateAdminProfileSuccess);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Updates the admin reset password asynchronous.
        /// </summary>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<bool> UpdateAdminResetPasswordAsync(long adminId, string password)
        {
            try
            {
                var admin = await _context.Admin.FirstOrDefaultAsync(x => x.AdminId == adminId);

                if (admin is null)
                {
                    return false;
                }

                admin.Password = password;

                _context.Admin.Update(admin);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
