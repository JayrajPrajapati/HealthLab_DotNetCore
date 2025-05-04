using HealthLayby.Models.AdminViewModels;
using HealthLayby.Models.Models;

namespace HealthLayby.Repositories.Repositories
{
    /// <summary>
    /// IReward Repository
    /// </summary>
    public interface IRewardsRepository
    {
        /// <summary>
        /// Rewards the grid list asynchronous.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        Task<Tuple<List<RewardsGridListResult>, int, int>> RewardGridListAsync(string sortColumn, string sortOrder, int pageSize, int pageIndex, string? searchText);

        /// <summary>
        /// Gets the reward model by identifier asynchronous.
        /// </summary>
        /// <param name="rewardId">The reward identifier.</param>
        /// <returns></returns>
        Task<RewardModel?> GetRewardModelByIdAsync(long rewardId);

        /// <summary>
        /// Saves the reward asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<(bool, string)> SaveRewardAsync(RewardModel model, long adminId);

        /// <summary>
        /// Deletes the reward asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="adminId">The admin identifier.</param>
        /// <returns></returns>
        Task<Tuple<bool, string>> DeleteRewardAsync(long id, long adminId);
    }
}
