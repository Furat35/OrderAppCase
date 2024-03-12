using EventBus.Message.Events;
using MassTransit;
using Ordering.Application.Services;

namespace Ordering.Logger.Consumers
{
    public class OrderLogConsumer : IConsumer<OrderLogEvent>
    {
        private readonly IOrderLoggerService _loggerService;

        public OrderLogConsumer(IOrderLoggerService loggerService)
        {
            _loggerService = loggerService;
        }

        public async Task Consume(ConsumeContext<OrderLogEvent> context)
        {
            await _loggerService.CreateLog(new() { OrderId = context.Message.OrderId, Message = context.Message.Message, Operation = context.Message.Operation });
        }
    }
}
