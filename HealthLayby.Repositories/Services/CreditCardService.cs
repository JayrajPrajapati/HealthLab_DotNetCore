using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels.CreditCard.Request;
using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// BankService
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Repositories.ICreditCardRepository" />
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    public class CreditCardService : BaseService, ICreditCardRepository
    {
        #region Constructor

        /// <summary>
        /// The stripe application service
        /// </summary>
        /// <param name="context">The context.</param>
        public CreditCardService(AppDbContext context) : base(context)
        {
            //  _stripeAppService = stripeAppService;
        }

        #endregion

        #region Mobile API's

        /// <summary>
        /// Saves the bank details.
        /// </summary>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="saveUpdateCreditCardRequest">The save update credit card request.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveUpdateCreditCardDetails(long customerId, SaveUpdateCreditCardRequest saveUpdateCreditCardRequest)
        {
            var message = string.Empty;
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var isCheckCreditCardExists = await _context.CustomerCreditCard.Where(x => x.CardNumber == saveUpdateCreditCardRequest.CardNumber && x.IsDeleted == false).FirstOrDefaultAsync();
                if (isCheckCreditCardExists is null)
                {
                    CustomerCreditCard creditCard = new CustomerCreditCard
                    {
                        CustomerId = customerId,
                        CardNumber = saveUpdateCreditCardRequest.CardNumber,
                        NameOnCard = saveUpdateCreditCardRequest.NameOnCard,
                        Month = saveUpdateCreditCardRequest.Month,
                        Year = saveUpdateCreditCardRequest.Year,
                        CCV = saveUpdateCreditCardRequest.CCV,
                        CreatedBy = customerId,
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    await _context.CustomerCreditCard.AddAsync(creditCard);
                    await _context.SaveChangesAsync();
                    message = ApiMessages.SuccessfullCreditCard;

                    await trans.CommitAsync();

                    if (creditCard.CreditCardId > 0)
                        return (true, message);
                    else
                        return (false, ApiMessages.InvalidNewCreditCard);

                }
                else
                {
                    isCheckCreditCardExists.UpdatedBy = customerId;
                    isCheckCreditCardExists.UpdatedOn = DateTime.UtcNow;
                    isCheckCreditCardExists.IsDeleted = true;

                    _context.CustomerCreditCard.Update(isCheckCreditCardExists);
                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();

                    CustomerCreditCard newCreditCard = new CustomerCreditCard
                    {
                        CustomerId = customerId,
                        CardNumber = saveUpdateCreditCardRequest.CardNumber,
                        NameOnCard = saveUpdateCreditCardRequest.NameOnCard,
                        Month = saveUpdateCreditCardRequest.Month,
                        Year = saveUpdateCreditCardRequest.Year,
                        CCV = saveUpdateCreditCardRequest.CCV,
                        CreatedBy = customerId,
                        CreatedOn = DateTime.UtcNow,
                        IsDeleted = false
                    };

                    await _context.CustomerCreditCard.AddAsync(newCreditCard);
                    await _context.SaveChangesAsync();
                    message = ApiMessages.UpdatedCreditCard;

                    if (newCreditCard.CreditCardId > 0)
                        return (true, message);
                    else
                        return (false, ApiMessages.InvalidOldCreditCard);

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