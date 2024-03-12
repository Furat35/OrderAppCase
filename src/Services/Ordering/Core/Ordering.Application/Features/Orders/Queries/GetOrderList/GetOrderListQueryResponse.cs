using Ordering.Application.Models.Dtos.Orders;

namespace Ordering.Application.Features.Orders.Queries.GetOrderList
{
    public class GetOrderListQueryResponse
    {
        public List<OrderListDto> Orders { get; set; }
    }
}
