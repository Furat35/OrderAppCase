using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.Application.Mail;
using Ordering.Application.Models.Dtos.Audit;
using Ordering.Application.Services;
using System.Text;

namespace Ordering.Infrastructure.BackgroundServices
{
    public class OrderLogEmailSender : IHostedService, IDisposable
    {
        private IEmailService _emailService;
        private readonly string _toEmailAddress;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public OrderLogEmailSender(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _toEmailAddress = "email@gmail.com";
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await SendOrderLogs();
            var now = DateTime.Now;
            var timeUntilNextExecution = DateTime.Today.AddDays(1).AddHours(2) - now;

            _timer = new Timer(_ => SendOrderLogs(), null, timeUntilNextExecution, TimeSpan.FromDays(1));
        }


        private async Task SendOrderLogs()
        {
            using var scope = _scopeFactory.CreateScope();
            var orderLoggerService = scope.ServiceProvider.GetRequiredService<IOrderLoggerService>();

            _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
            var orderLogs = orderLoggerService.GetLogs(DateTime.Today);
            var emailBody = GenerateEmailBody(orderLogs);

            await _emailService.SendEmailAsync(_toEmailAddress, "Daily Order Logs", emailBody);
        }

        private string GenerateEmailBody(List<AuditListDto> orderLogs)
        {
            StringBuilder bodyBuilder = new StringBuilder();
            foreach (var orderLog in orderLogs)
            {
                string logInfo = $"{orderLog.Id}, {orderLog.Message}, {Enum.GetName(orderLog.Operation)}\n";
                bodyBuilder.AppendLine(logInfo);
            }

            return bodyBuilder.ToString();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
