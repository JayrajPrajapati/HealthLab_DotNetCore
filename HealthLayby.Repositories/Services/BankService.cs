using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels.Bank.Request;
using HealthLayby.Models.ApiViewModels.Bank.Response;
using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// BankService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.IBankRepository" />
    public class BankService : BaseService, IBankRepository
    {
        #region Private Variable 

        /// <summary>
        /// The stripe application service
        /// </summary>
        /// <param name="context">The context.</param>
        public BankService(AppDbContext context) : base(context)
        {
          //  _stripeAppService = stripeAppService;
        }

        #endregion

        #region Mobile API's

        /// <summary>
        /// Determines whether [is validate BSB number] [the specified BSB number].
        /// </summary>
        /// <param name="BSBNumber">The BSB number.</param>
        /// <returns></returns>
        public async Task<bool> IsValidateBSBNumberForAPI(string BSBNumber)
        {
            try
            {
                return await _context.BankBSBDirectory.AnyAsync(x => x.BSBNumber == BSBNumber);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the bank detail after BSB validation for API.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<(bool, string, ValidateBSBNumberResponse)> GetBankDetailAfterBSBValidationForAPI(ValidateBSBNumberRequest model)
        {
            var message = string.Empty;
            Bank bank = new Bank();
            ValidateBSBNumberResponse validateBSBNumberResponse = new ValidateBSBNumberResponse();
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var data = _context.BankBSBDirectory.Where(x => x.BSBNumber == model.BSBNumber).FirstOrDefault();
                if (data is not null)
                {
                    validateBSBNumberResponse.BankBSBDirectoryId = data.BankBSBDirectoryId;
                    validateBSBNumberResponse.BankLocation = data.BankStreetAddress;
                    bank = _context.Bank.Where(y => y.BankId == data.BankId).FirstOrDefault();
                    if (bank is not null)
                    {
                        validateBSBNumberResponse.BankName = bank.BankName;
                    }
                }

                if (validateBSBNumberResponse.BankBSBDirectoryId > 0)
                    return (true, ApiMessages.ValidBSBNumber, validateBSBNumberResponse);
                else
                    return (false, ApiMessages.InValidBSBNumber, null);
            }
            catch
            {
                await trans.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Saves the bank details.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="saveBankDetailsRequest">The save bank details request.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveBankDetails(long customerId, SaveBankDetailsRequest saveBankDetailsRequest)
        {
            var message = string.Empty;            
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var BankAccount = await _context.CustomerBank.Where(x => x.AccountNumber == saveBankDetailsRequest.AccountNumber && x.BankName == saveBankDetailsRequest.BankName).FirstOrDefaultAsync();

                if (BankAccount is null)
                {
                    CustomerBank customerBank = new CustomerBank
                    {
                        CustomerId = customerId,
                        AccountNumber = saveBankDetailsRequest.AccountNumber,
                        BankName = saveBankDetailsRequest.BankName,
                        BankLocation = saveBankDetailsRequest.BankLocation,
                        FullName = saveBankDetailsRequest.FullName,
                        IsVerified = true,
                        CreatedBy = customerId,
                        CreatedOn = DateTime.UtcNow
                    };

                    await _context.CustomerBank.AddAsync(customerBank);
                    await _context.SaveChangesAsync();
                    message = ApiMessages.SuccessfullBank;

                    await trans.CommitAsync();

                    if (customerBank.CustomerBankId > 0)
                        return (true, message);
                    else
                        return (false, ApiMessages.InvalidBank);

                }
                else
                {
                    BankAccount.CustomerId = customerId;
                    BankAccount.AccountNumber = saveBankDetailsRequest.AccountNumber;
                    BankAccount.BankName = saveBankDetailsRequest.BankName;
                    BankAccount.BankLocation = saveBankDetailsRequest.BankLocation;
                    BankAccount.FullName = saveBankDetailsRequest.FullName;
                    BankAccount.IsVerified = true;
                    BankAccount.UpdatedBy = customerId;
                    BankAccount.UpdatedOn = DateTime.UtcNow;

                    _context.CustomerBank.Update(BankAccount);
                    await _context.SaveChangesAsync();
                    message = ApiMessages.SuccessfullBank;

                    await trans.CommitAsync();

                    if (BankAccount.CustomerBankId > 0)
                        return (true, message);
                    else
                        return (false, ApiMessages.InvalidBank);

                }
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