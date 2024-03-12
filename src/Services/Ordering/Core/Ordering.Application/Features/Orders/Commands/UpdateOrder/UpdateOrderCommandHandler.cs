using AutoMapper;
using EventBus.Message.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Models.Dtos.Orders;
using Ordering.Application.Services;
using Shared.Enums;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommandRequest, UpdateOrderCommandResponse>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public UpdateOrderCommandHandler(IOrderService orderService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<UpdateOrderCommandResponse> Handle(UpdateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<OrderUpdateDto>(request);
            var isSuccess = await _orderService.Update(order);
            if (isSuccess)
            {
                var orderLog = new OrderLogEvent() { OrderId = request.Id, Message = $"Order with id: {{ {request.Id} }} is updated!", Operation = OperationType.Update };
                await _publishEndpoint.Publish(orderLog);
            }

            return new UpdateOrderCommandResponse { IsSuccess = isSuccess };
        }
    }
}
