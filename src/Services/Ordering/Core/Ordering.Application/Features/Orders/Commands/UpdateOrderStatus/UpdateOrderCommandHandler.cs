using EventBus.Message.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Services;
using Shared.Enums;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommandRequest, UpdateOrderStatusCommandResponse>
    {
        private readonly IOrderService _orderService;
        private readonly IPublishEndpoint _publishEndpoint;

        public UpdateOrderStatusCommandHandler(IOrderService orderService, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<UpdateOrderStatusCommandResponse> Handle(UpdateOrderStatusCommandRequest request, CancellationToken cancellationToken)
        {
            var isSuccess = await _orderService.ChangeOrderStatus(request.OrderId, request.Status);
            if (isSuccess)
            {
                var orderLog = new OrderLogEvent() { OrderId = request.OrderId, Message = $"Order id with: {{ {request.OrderId} }} changed status to {request.Status}", Operation = OperationType.Update };
                await _publishEndpoint.Publish(orderLog);
            }

            return new UpdateOrderStatusCommandResponse { IsSuccess = isSuccess };
        }
    }
}
