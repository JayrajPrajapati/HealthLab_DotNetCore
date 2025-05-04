namespace HealthLayby.API.Infrastructure.EmailHelper
{
    public interface IEmailService
    {
        Task SendEmail(string[] toEmail, string subject, string mailBody, bool isBodyHTML = true);
    }
}
