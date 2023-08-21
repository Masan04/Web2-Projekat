using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekat.Dto;
using Projekat.Interfaces;

namespace Projekat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("create")]
        [Authorize(Roles ="buyer")]
        public IActionResult CreateOrder([FromBody]OrderDto orderDto)
        {
            OrderDto order = _orderService.CreateOrder(orderDto);
            if(order == null)
            {
                return BadRequest("Error occurred while creating an order!");

            }
            return Ok(order);

        }
        [HttpGet("buyer/{buyerId}")]
        [Authorize(Roles = "buyer")]
        public IActionResult GetOrdersByBuyerId(long buyerId)
        {
            List<OrderCancelCheckDto> orders = new List<OrderCancelCheckDto>();
            orders = _orderService.GetOrdersByBuyerId(buyerId);
            if (orders == null)
                return BadRequest("Error occurred while getting orders!");
            return Ok(orders);
        }

        [HttpGet("newOrders/{sellerId}")]
        [Authorize(Roles = "seller")]
        public IActionResult GetNewOrdersBySellerId(long sellerId)
        {
            List<OrderDto> orders = new List<OrderDto>();
            orders = _orderService.GetNewOrdersBySellerId(sellerId);
            if (orders == null)
            {
                return BadRequest("Error occurred while getting orders!");
            }
            return Ok(orders);
        }

        [HttpGet("pastOrders/{sellerId}")]
        [Authorize(Roles = "seller")]
        public IActionResult GetPastOrdersBySellerId(long sellerId)
        {
            List<OrderDto> orders = new List<OrderDto>();
            orders = _orderService.GetPastOrdersBySellerId(sellerId);
            if (orders == null)
            {
                return BadRequest("Error occurred while getting orders!");
            }
            return Ok(orders);
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAll()
        {
            List<OrderDto> orders = new List<OrderDto>();
            orders = _orderService.GetAll();
            if (orders == null)
            {
                return BadRequest("Error occurred while getting orders!");
            }
            return Ok(orders);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "buyer")]
        public IActionResult DeleteOrder(long id)
        {
            OrderDto order = _orderService.DeleteOrder(id);
            if (order == null)
                return BadRequest("Error occurred while deleting an order!");
            return Ok(order);
        }
    }
}
