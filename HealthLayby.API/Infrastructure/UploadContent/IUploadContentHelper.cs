namespace HealthLayby.API.Infrastructure.UploadContent
{
    /// <summary>
    /// IUploadContentHelper
    /// </summary>
    public interface IUploadContentHelper
    {
        /// <summary>
        /// Updates the customer profile.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        Task<(bool, string)> UpdateCustomerProfile(IFormFile file);
    }
}
