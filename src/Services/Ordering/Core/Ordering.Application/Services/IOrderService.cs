using Ordering.Application.Models.Dtos.Orders;

namespace Ordering.Application.Services
{
    public interface IOrderService
    {
        Task<Guid> Create(OrderCreateDto order);
        Task<bool> Update(OrderUpdateDto order);
        Task<bool> Delete(Guid orderId);
        Task<List<OrderListDto>> Get();
        Task<OrderListDto> Get(Guid orderId);
        Task<bool> ChangeOrderStatus(Guid orderId, string status);
    }
}
