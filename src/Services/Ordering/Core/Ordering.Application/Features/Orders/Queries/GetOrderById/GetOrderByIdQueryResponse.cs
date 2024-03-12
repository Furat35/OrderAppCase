using Ordering.Application.Models.Dtos.Orders;

namespace Ordering.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryResponse
    {
        public OrderListDto Order { get; set; }
    }
}
