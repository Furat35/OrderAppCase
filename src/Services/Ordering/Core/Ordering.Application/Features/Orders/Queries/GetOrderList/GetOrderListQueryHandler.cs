using AutoMapper;
using MediatR;
using Ordering.Application.Services;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQueryRequest, GetOrderListQueryResponse>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public GetOrderListQueryHandler(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task<GetOrderListQueryResponse> Handle(GetOrderListQueryRequest request, CancellationToken cancellationToken)
        {
            var orders = await _orderService.Get();
            return _mapper.Map<GetOrderListQueryResponse>(orders);
        }
    }
}
