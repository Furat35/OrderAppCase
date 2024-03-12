using AutoMapper;
using MediatR;
using Ordering.Application.Services;

namespace Ordering.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderService.Get(request.OrderId);
            return _mapper.Map<GetOrderByIdQueryResponse>(order);
        }
    }
}
