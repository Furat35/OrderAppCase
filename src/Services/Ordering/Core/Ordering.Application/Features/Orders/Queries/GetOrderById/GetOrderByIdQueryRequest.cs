using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryRequest : IRequest<GetOrderByIdQueryResponse>
    {
        public Guid OrderId { get; set; }
    }
}
