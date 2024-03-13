using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Orders.Commands.CreateOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrderStatus;
using Ordering.Application.Features.Orders.Queries.GetOrderById;
using Ordering.Application.Features.Orders.Queries.GetOrderList;
using Ordering.Application.Models.Dtos.Orders;
using Ordering.Domain.Enums;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get orders
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(OrderListDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _mediator.Send(new GetOrderListQueryRequest());
            return Ok(response);
        }

        /// <summary>
        /// Get orders with give id 
        /// </summary>
        /// <param name="id">Order id</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderListDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrders(string id)
        {
            var response = await _mediator.Send(new GetOrderByIdQueryRequest { OrderId = Guid.Parse(id) });
            return Ok(response);
        }

        /// <summary>
        /// Create order 
        /// </summary>
        /// <param name="order">Order properties needed to create the order</param>
        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateOrder([FromForm] CreateOrderCommandRequest order)
        {
            var response = await _mediator.Send(order);
            return Ok(response);
        }

        /// <summary>
        /// Update customer 
        /// </summary>
        /// <param name="order">Order properties needed to update the customer</param>
        [HttpPut("update")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrderCommandRequest order)
        {
            var response = await _mediator.Send(order);
            return Ok(response);
        }

        /// <summary>
        /// Change order status to Proccessing 
        /// </summary>
        /// <param name="orderId">Order id</param>
        [HttpPut("setToProccessing/{orderId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetOrderToProccessing(string orderId)
        {
            var response = await _mediator.Send(new UpdateOrderStatusCommandRequest { OrderId = Guid.Parse(orderId), Status = Enum.GetName(OrderStatus.Processing) });
            return Ok(response);
        }

        /// <summary>
        /// Change order status to completed 
        /// </summary>
        /// <param name="order">Order id</param>
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("setToCompleted/{orderId}")]
        public async Task<IActionResult> SetOrderToCompleted(string orderId)
        {
            var response = await _mediator.Send(new UpdateOrderStatusCommandRequest { OrderId = Guid.Parse(orderId), Status = Enum.GetName(OrderStatus.Completed) });
            return Ok(response);
        }

        /// <summary>
        /// Delete order 
        /// </summary>
        /// <param name="id">Order id</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var response = await _mediator.Send(new DeleteOrderCommandRequest { OrderId = Guid.Parse(id) });
            return Ok(response);
        }
    }
}
