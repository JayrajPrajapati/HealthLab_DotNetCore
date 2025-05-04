using HealthLayby.Helpers.Constant;
using HealthLayby.Models.ApiViewModels.Transaction.Response;
using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// Transaction Service
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.ITransactionRepository" />
    public class TransactionService : BaseService, ITransactionRepository
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public TransactionService(AppDbContext context) : base(context)
        {
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Gets the transaction list.
        /// </summary>
        /// <returns></returns>
        public async Task<(bool, string, TransactionResponse)> GetTransactionList()
        {
            List<TransactionDetails> transactionList = new List<TransactionDetails>();
            List<Service> services = new List<Service>();
            TransactionResponse transactionResponse = new TransactionResponse();
            string message = string.Empty;
            int countIds = 1;
            decimal amount = 10;
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                services = await _context.Service.Where(x => x.IsActive == true && !x.IsDeleted).ToListAsync();
                if (services is not null && services.Count() > 0)
                {
                    foreach (var service in services)
                    {
                        TransactionDetails details = new TransactionDetails
                        {
                            Id = countIds,
                            ServiceName = service.ServiceName,
                            PlanSku = Guid.NewGuid(),
                            Amount = amount,
                            DateTime = DateTime.UtcNow
                        };
                        transactionList.Add(details);
                        countIds++;
                        amount += 10;

                    }

                    transactionResponse.TransactionDetails = transactionList;
                    transactionResponse.TransactionPDF = "Transactionlist.pdf";
                    message = MessageConstant.TransactionListSuccess;
                    return (true, MessageConstant.TransactionListSuccess, transactionResponse);
                }
                return (false, MessageConstant.TransactionListNotFound, null);

            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                throw;
            }
        } 
        #endregion
    }
}
