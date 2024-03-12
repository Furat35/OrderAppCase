using AutoMapper;
using Ordering.Application.Features.Orders.Commands.CreateOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrderById;
using Ordering.Application.Features.Orders.Queries.GetOrderList;
using Ordering.Application.Models.Dtos.Orders;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderCommandRequest, OrderCreateDto>();
            CreateMap<OrderCreateDto, Order>();

            CreateMap<UpdateOrderCommandRequest, OrderUpdateDto>();
            CreateMap<OrderUpdateDto, Order>();

            CreateMap<Order, OrderListDto>();
            CreateMap<OrderListDto, GetOrderByIdQueryResponse>()
                .ForMember(_ => _.Order, opt => opt.MapFrom(src => src));

            CreateMap<OrderListDto, GetOrderListQueryRequest>();
            CreateMap<List<OrderListDto>, GetOrderListQueryResponse>()
                .ForMember(_ => _.Orders, opt => opt.MapFrom(src => src));

        }
    }
}
