using HealthLayby.Helpers.Constant;
using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.Context;
using HealthLayby.Models.Models;
using HealthLayby.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HealthLayby.Repositories.Services
{
    /// <summary>
    /// Reward Service
    /// </summary>
    /// <seealso cref="HealthLayby.Repositories.Services.BaseService" />
    /// <seealso cref="HealthLayby.Repositories.Repositories.IRewardsRepository" />
    public class RewardsService : BaseService, IRewardsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RewardsService" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public RewardsService(AppDbContext context) : base(context)
        {
        }

        #region Public Methods

        /// <summary>
        /// Deletes the reward asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<Tuple<bool, string>> DeleteRewardAsync(long id, long adminId)
        {
            try
            {
                var reward = await _context.Rewards.Where(q => q.RewardId == id && !q.IsDeleted).FirstOrDefaultAsync();

                if (reward is null)
                {
                    return Tuple.Create(false, MessageConstant.RewardNotFound);
                }

                reward.IsDeleted = true;
                reward.DeletedOn = DateTime.UtcNow;
                reward.DeletedBy = adminId;

                _context.Rewards.Update(reward);
                await _context.SaveChangesAsync();

                return Tuple.Create(true, MessageConstant.RewardDeleteSuccess);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the reward model by identifier asynchronous.
        /// </summary>
        /// <param name="rewardId">The reward identifier.</param>
        /// <returns></returns>
        public async Task<RewardModel?> GetRewardModelByIdAsync(long rewardId)
        {
            try
            {
                return await _context.Rewards.Where(q => !q.IsDeleted && q.RewardId == rewardId)
                                         .Select(q => new RewardModel
                                         {
                                             CategoryId = q.CategoryId,
                                             Discount = q.Discount,
                                             DiscountType = q.DiscountType,
                                             IsActive = q.IsActive,
                                             MerchantId = q.MerchantId,
                                             Month = q.Month,
                                             RewardId = q.RewardId
                                         }).FirstOrDefaultAsync();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Rewards the grid list asynchronous.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public async Task<Tuple<List<RewardsGridListResult>, int, int>> RewardGridListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText)
        {
            try
            {
                var paramTotalRecords = new OutputParameter<int?>();
                var paramTotalFilteredRecords = new OutputParameter<int?>();

                var result = await _context.GetProcedures().RewardsGridListAsync
                (
                    SortColumn: sortColumn,
                    SortOrder: sortOrder,
                    PageSize: pageSize,
                    PageIndex: pageIndex,
                    SearchText: searchText,
                    TotalRecords: paramTotalRecords,
                    TotalFilteredRecords: paramTotalFilteredRecords
                );

                return Tuple.Create(result, paramTotalRecords.Value ?? 0, paramTotalFilteredRecords.Value ?? 0);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Saves the reward asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveRewardAsync(RewardModel model, long adminId)
        {
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                var message = string.Empty;
                var isMerchantAvailable = await _context.Merchant.AnyAsync(q => !q.IsDeleted && q.MerchantId == model.MerchantId);

                if (!isMerchantAvailable)
                {
                    return (false, MessageConstant.MerchantNotFound);
                }

                var isCategoryAvailable = await _context.Category.AnyAsync(q => !q.IsDeleted && q.CategoryId == model.CategoryId);

                if (!isCategoryAvailable)
                {
                    return (false, MessageConstant.CategoryNotFound);
                }

                var reward = await _context.Rewards.FirstOrDefaultAsync(q => !q.IsDeleted && q.RewardId == model.RewardId);

                if (reward is not null)
                {
                    reward.CategoryId = model.CategoryId;
                    reward.Discount = model.Discount;
                    reward.DiscountType = model.DiscountType;
                    reward.IsActive = model.IsActive;
                    reward.MerchantId = model.MerchantId;
                    reward.Month = model.Month;
                    reward.UpdatedBy = adminId;
                    reward.UpdatedOn = DateTime.UtcNow;

                    _context.Rewards.Update(reward);
                    await _context.SaveChangesAsync();
                    message = MessageConstant.RewardUpdateSuccess;
                }
                else
                {
                    reward = new Rewards
                    {
                        CategoryId = model.CategoryId,
                        CreatedBy = adminId,
                        CreatedOn = DateTime.UtcNow,
                        Discount = model.Discount,
                        DiscountType = model.DiscountType,
                        IsActive = model.IsActive,
                        MerchantId = model.MerchantId,
                        Month = model.Month
                    };

                    await _context.Rewards.AddAsync(reward);
                    await _context.SaveChangesAsync();

                    message = MessageConstant.RewardCreateSuccess;

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

        #endregion
    }
}
