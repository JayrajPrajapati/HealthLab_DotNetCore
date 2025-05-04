using HealthLayby.Models.ApiViewModels.Bank.Request;
using HealthLayby.Models.ApiViewModels.Bank.Response;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// IBankRepository
    /// </summary>
    public interface IBankRepository
    {
        /// <summary>
        /// Determines whether [is validate BSB number] [the specified BSB number].
        /// </summary>
        /// <param name="BSBNumber">The BSB number.</param>
        /// <returns></returns>
        Task<bool> IsValidateBSBNumberForAPI(string BSBNumber);


        /// <summary>
        /// Gets the bank detail after BSB validation for API.
        /// </summary>
        /// <param name="validateBSBNumberRequest">The validate BSB number request.</param>
        /// <returns></returns>
        Task<(bool, string, ValidateBSBNumberResponse)> GetBankDetailAfterBSBValidationForAPI(ValidateBSBNumberRequest validateBSBNumberRequest);

        /// <summary>
        /// Saves the bank details.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="saveBankDetailsRequest">The save bank details request.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveBankDetails(long customerId, SaveBankDetailsRequest saveBankDetailsRequest);

    }
}
