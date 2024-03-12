using EventBus.Message.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Services;
using Shared.Enums;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommandRequest, DeleteOrderCommandResponse>
    {
        private readonly IOrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteOrderCommandHandler(IOrderService orderService, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<DeleteOrderCommandResponse> Handle(DeleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var isSuccess = await _orderService.Delete(request.OrderId);
            if (isSuccess)
            {
                var orderLog = new OrderLogEvent() { OrderId = request.OrderId, Message = $"Order with id: {{ {request.OrderId} }} is deleted!", Operation = OperationType.Delete };
                await _publishEndpoint.Publish(orderLog);
            }

            return new DeleteOrderCommandResponse { IsSuccess = isSuccess };
        }
    }
}
