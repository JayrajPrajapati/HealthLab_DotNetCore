using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.Context;
using HealthLayby.Models.MerchentViewModels;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories.MerchantRepositories;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Repositories.Services.MerchantServices
{
    public class MerchantFaqService : BaseService, IMerchantFaqWebRepository
    {
        #region Private Variable 
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchantFaqService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public MerchantFaqService(AppDbContext context) : base(context)
        {

        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the merchant old password.
        /// </summary>
        /// <returns></returns>
        public async Task<List<FAQMerchantModel>> GetMerchantFaq()
        {
            try
            {
                return await _context.FAQ.Where(q => !q.IsDeleted)
                                            .Select(q => new FAQMerchantModel
                                            {
                                                FAQId = q.FAQId,
                                                Question = q.Question,
                                                Answer = q.Answer
                                            }).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///   Gets the FAQ by identifier asynchronous.
        /// </summary>
        /// <param name="faqId">The FAQ identifier.</param>
        /// <returns></returns>
        public async Task<FAQMerchantModel?> GetFAQByIdAsync(long faqId)
        {
            try
            {
                return await _context.FAQ.Where(q => q.FAQId == faqId && !q.IsDeleted)
                                              .Select(q => new FAQMerchantModel
                                              {
                                                  FAQId = q.FAQId,
                                                  Question = q.Question,
                                                  Answer = q.Answer
                                              }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///   Saves the FAQ asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveFAQAsync(FAQMerchantModel model, long merchantId)
        {
            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    string message = string.Empty;
                    var faq = await _context.FAQ.FirstOrDefaultAsync(x => x.FAQId == model.FAQId);

                    if (faq is null)
                    {
                        faq = new FAQ
                        {
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = merchantId,
                            Question = model.Question,
                            Answer = model.Answer
                        };

                        await _context.FAQ.AddAsync(faq);
                        await _context.SaveChangesAsync();

                        message = MessageConstant.CreateFAQSuccess;
                    }
                    else
                    {
                        faq.UpdatedOn = DateTime.UtcNow;
                        faq.UpdatedBy = merchantId;
                        faq.Question = model.Question;
                        faq.Answer = model.Answer;

                        _context.FAQ.Update(faq);
                        await _context.SaveChangesAsync();

                        message = MessageConstant.UpdateFAQSuccess;
                    }

                    await trans.CommitAsync();
                    return (true, message);
                }
                catch
                {
                    await trans.RollbackAsync();
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes the FAQ asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="merchantId">The merchant identifier.</param>
        /// <param name="rejectReason">The reject reason.</param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> DeleteFAQAsync(long id, long merchantId, string rejectReason)
        {
            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var faq = await _context.FAQ.FirstOrDefaultAsync(q => q.FAQId == id && !q.IsDeleted);

                    if (faq is null)
                    {
                        return Tuple.Create(false, MessageConstant.FAQNotFound);
                    }

                    faq.IsDeleted = true;
                    faq.DeletedOn = DateTime.UtcNow;
                    faq.DeletedBy = merchantId;
                    faq.RejectReason = !string.IsNullOrWhiteSpace(rejectReason) ? rejectReason : faq.RejectReason;

                    _context.FAQ.Update(faq);
                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                    return Tuple.Create(true, MessageConstant.DeleteFAQSuccess);
                }
                catch
                {
                    await trans.RollbackAsync();
                    throw;
                }
            }
        }

        #endregion
    }
}
