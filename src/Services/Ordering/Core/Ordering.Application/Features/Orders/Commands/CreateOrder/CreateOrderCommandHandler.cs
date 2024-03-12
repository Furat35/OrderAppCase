using AutoMapper;
using EventBus.Message.Events;
using MassTransit;
using MediatR;
using Ordering.Application.Models.Dtos.Orders;
using Ordering.Application.Services;
using Shared.Enums;

namespace Ordering.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateOrderCommandHandler(IOrderService orderService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _orderService = orderService;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<OrderCreateDto>(request);
            var orderId = await _orderService.Create(order);
            var orderLog = new OrderLogEvent() { OrderId = orderId, Message = $"Order with id: {{ {orderId} }} is created!", Operation = OperationType.Create };
            await _publishEndpoint.Publish(orderLog);

            return new CreateOrderCommandResponse { OrderId = orderId };
        }
    }
}
