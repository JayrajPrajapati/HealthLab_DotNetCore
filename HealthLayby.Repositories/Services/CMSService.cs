using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.ApiViewModels.ContentManagement.Response;
using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using static HealthLayby.Helpers.Constant.Enum;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// CMS Service
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.ICMSRepository" />
    public class CMSService : BaseService, ICMSRepository
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CMSService"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CMSService(AppDbContext context) : base(context)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves the content asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveContentAsync(CMSModel model, long adminId)
        {
            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (model.IsActive)
                    {
                        if (string.IsNullOrWhiteSpace(model.Description))
                        {
                            return (true, MessageConstant.DescriptionInValid);
                        }
                    }

                    var contentManagement = await _context.ContentManagement.FirstOrDefaultAsync(x => x.PageCode == (int)model.PageCode);

                    if (contentManagement is null)
                    {
                        contentManagement = new ContentManagement
                        {
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = adminId,
                            PageCode = (int)model.PageCode,
                            Description = model.Description,
                            IsActive = model.IsActive,
                            Title =  model.Title
                        };

                        await _context.ContentManagement.AddAsync(contentManagement);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        contentManagement.UpdatedOn = DateTime.UtcNow;
                        contentManagement.UpdatedBy = adminId;
                        contentManagement.Description = model.Description;
                        contentManagement.IsActive = model.IsActive;
                        contentManagement.Title = model.Title;

                        _context.ContentManagement.Update(contentManagement);
                        await _context.SaveChangesAsync();
                    }

                    await trans.CommitAsync();

                    if (model.PageCode == ContentManagementEnum.PrivacyPolicy)
                    {
                        return (true, MessageConstant.UpdatePrivacyPolicySuccess);
                    }
                    else if (model.PageCode == ContentManagementEnum.Termsconditions)
                    {
                        return (true, MessageConstant.UpdateTermConditionSuccess);
                    }
                    else if (model.PageCode == ContentManagementEnum.WhyIncludingAdminFee)
                    {
                        return (true, MessageConstant.UpdateWhyIncludingAdminFeesSuccess);
                    }
                    else
                    {
                        return (true, MessageConstant.CommonErrorMessage);
                    }
                }
                catch
                {
                    await trans.RollbackAsync();
                    throw;
                }
            }
        }

        /// <summary>
        /// Saves the FAQ asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveFAQAsync(FAQModel model, long adminId)
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
                            CreatedBy = adminId,
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
                        faq.UpdatedBy = adminId;
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
        /// Gets the FAQ by identifier asynchronous.
        /// </summary>
        /// <param name="faqId">The FAQ identifier.</param>
        /// <returns></returns>
        public async Task<FAQModel?> GetFAQByIdAsync(long faqId)
        {
            try
            {
                return await _context.FAQ.Where(q => q.FAQId == faqId && !q.IsDeleted)
                                              .Select(q => new FAQModel
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
        /// Deletes the FAQ asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <param name="rejectReason">The reject reason.</param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> DeleteFAQAsync(long id, long adminId, string rejectReason)
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
                    faq.DeletedBy = adminId;
                    faq.RejectReason = !string.IsNullOrWhiteSpace(rejectReason) ? rejectReason: faq.RejectReason;

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

        /// <summary>
        /// Gets the FAQ list asynchronous.
        /// </summary>
        /// <returns></returns>
        public async Task<List<FAQModel>> GetFAQListAsync()
        {
            try
            {
                return await _context.FAQ.Where(x => x.IsDeleted == false)
                                .Select(x => new FAQModel
                                {
                                    FAQId = x.FAQId,
                                    Question = x.Question,
                                    Answer = x.Answer
                                }).ToListAsync(); ;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the page content by page code asynchronous.
        /// </summary>
        /// <param name="pageCode">The page code.</param>
        /// <returns></returns>
        public async Task<CMSModel?> GetPageContentByPageCodeAsync(int pageCode)
        {
            try
            {
                return await _context.ContentManagement.Where(x => x.PageCode == pageCode)
                                                       .Select(x => new CMSModel
                                                       {
                                                           IsActive = x.IsActive,
                                                           Description = x.Description,
                                                           Title =x.Title,
                                                           PageCode=(ContentManagementEnum)x.PageCode
                                                       }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the termsand condition content by page code asynchronous.
        /// </summary>
        /// <param name="pageCode">The page code.</param>
        /// <returns></returns>
        public async Task<CMSModel?> GetTermsandConditionContentByPageCodeAsync(int pageCode)
        {
            try
            {
                return await _context.ContentManagement.Where(x => x.PageCode == pageCode)
                                                       .Select(x => new CMSModel
                                                       {
                                                           IsActive = x.IsActive,
                                                           Description = x.Description,
                                                           Title = x.Title
                                                       }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the why including admin fee content by page code asynchronous.
        /// </summary>
        /// <param name="pageCode">The page code.</param>
        /// <returns></returns>
        public async Task<CMSModel?> GetWhyIncludingAdminFeeContentByPageCodeAsync(int pageCode)
        {
            try
            {
                return await _context.ContentManagement.Where(x => x.PageCode == pageCode)
                                                       .Select(x => new CMSModel
                                                       {
                                                           IsActive = x.IsActive,
                                                           Description = x.Description,
                                                           Title = x.Title
                                                       }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        #region Mobile APIs

        /// <summary>
        /// Gets the content by page code asynchronous for API.
        /// </summary>
        /// <param name="contentManagementEnum">The content management enum.</param>
        /// <returns></returns>
        public async Task<ContentResponse?> GetContentByPageCodeAsyncForAPI(ContentManagementEnum contentManagementEnum)
        {
            try
            {
                return await _context.ContentManagement.Where(x =>
                                                                x.PageCode == (int)contentManagementEnum
                                                                && x.IsActive
                                                                )
                                                       .Select(x => new ContentResponse
                                                       {
                                                           contentType = (ContentManagementEnum?)x.PageCode,
                                                           Title = x.Title,
                                                           Description = x.Description
                                                       }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the FAQ list asynchronous for API.
        /// </summary>
        /// <returns></returns>
        public async Task<List<FAQResponse>> GetFAQListAsyncForAPI()
        {
            try
            {
                return await _context.FAQ.Where(x => !x.IsDeleted)
                                .Select(x => new FAQResponse
                                {
                                    Question = x.Question,
                                    Answer = x.Answer
                                }).ToListAsync(); ;
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #endregion
    }
}
