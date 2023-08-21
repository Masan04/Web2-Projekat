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
    }
}
