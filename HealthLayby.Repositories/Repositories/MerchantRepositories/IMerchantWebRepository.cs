using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Models.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HealthLayby.Repositories.Repositories.MerchantRepositories
{
    /// <summary>
    /// IMerchantWebRepository
    /// </summary>
    public interface IMerchantWebRepository
    {
        /// <summary>
        /// Gets the merchant by email address asynchronous.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <returns></returns>
        Task<Merchant?> GetMerchantByEmailAddressAsync(string emailAddress);

        /// <summary>
        /// Gets the unused token for merchant asynchronous.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<Guid?> GetUnusedTokenForMerchantAsync(long merchantId);

        /// <summary>
        /// Adds the forgot password token for merchant asynchronous.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> AddForgotPasswordTokenForMerchantAsync(long merchantId, Guid token);

        /// <summary>
        /// Checks the merchant token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> CheckMerchantTokenAsync(Guid token);

        /// <summary>
        /// Gets the merchant identifier by token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<long?> GetMerchantIdByTokenAsync(Guid token);

        /// <summary>
        /// Gets the merchant by identifier asynchronous.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <returns></returns>
        Task<MerchantModel?> GetMerchantByIdAsync(long merchantId);

        /// <summary>
        /// Updates the merchant reset password asynchronous.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<bool> UpdateMerchantResetPasswordAsync(long merchantId, string password);

        /// <summary>
        /// Updates the merchant change date asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> UpdateMerchantChangeDateAsync(Guid token);

        /// <summary>
        /// Determines whether [is merchant exists] [the specified email].
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="firstName">The first name.</param>
        /// <returns></returns>
        Task<long> IsMerchantExists(string email, string firstName);

        /// <summary>
        /// Saves the merchant asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<(bool, string,Merchant)> SaveMerchantAsync(RegisterMerchantModel model, long adminId, string? password = null);

        /// <summary>
        ///   Gets the active category dropdown list asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<SelectListItem>> GetActiveCategoryDropdownListAsync();

        /// <summary>
        /// Gets the active service dropdown list asynchronous.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns></returns>
        Task<List<SelectListItem>> GetActiveServiceDropdownListAsync(long categoryId);

        /// <summary>
        /// Adds the generated otp.
        /// </summary>
        /// <param name="randomOTP">The random otp.</param>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> AddGeneratedOTP(string randomOTP, Guid token);

        /// <summary>
        /// Gets the generated otp.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<long> GetGeneratedOTP(Guid token);

        /// <summary>
        /// Updateds the otp.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        Task<bool> UpdatedOTP(Guid token);

        /// <summary>
        /// Updates the merchant clinic by identifier asynchronous.
        /// </summary>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="otp">The otp.</param>
        /// <returns></returns>
        Task<bool> UpdateMerchantClinicByIdAsync(long merchantId,long otp);

        /// <summary>
        /// Adds the merchant bank.
        /// </summary>
        /// <param name="merchantBankDetails">The merchant bank details.</param>
        /// <returns></returns>
        Task<bool> AddMerchantBank(MerchantBankDetails merchantBankDetails);
    }
}