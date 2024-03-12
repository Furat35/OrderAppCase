namespace Ordering.Application.Mail
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toAddress, string subject, string body, bool isHtml = false);
    }
}
