using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekat.Dto;
using Projekat.Interfaces;
using Projekat.Models;

namespace Projekat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "seller")]
        public IActionResult CreateItem([FromBody] ItemDto itemDto)
        {
            ItemDto item = _itemService.CreateItem(itemDto);
            if (item == null)
            {
                return BadRequest("Error occurred while creating an item!");
            }
            return Ok(item);
        }

        [HttpGet("{sellerId}")]
        [Authorize(Roles = "seller")]
        public IActionResult GetItemsBySellerId(long sellerId)
        {
            List<ItemDto> items = new List<ItemDto>();
            items = _itemService.GetItemsBySellerId(sellerId);
            if (items == null)
            {
                return BadRequest("Error occurred while getting an item!");
            }
            return Ok(items);
        }

        [HttpGet("byOrder/{orderId}")]
        [Authorize(Roles = "seller,admin,buyer")]
        public IActionResult GetItemsByOrderId(long orderId)
        {
            List<ItemDto> items = new List<ItemDto>();
            items = _itemService.GetItemsByOrderId(orderId);
            if (items == null)
            {
                return BadRequest("Error occurred while getting an item!");
            }
            return Ok(items);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="seller")]
        public IActionResult DeleteItem(long id)
        {
            bool result = _itemService.DeleteItem(id);
            if (result == false)
            {
                return BadRequest("Error occured while deleting an item!");
            }
            return Ok();

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "seller")]
        public IActionResult UpdateItem(long id, [FromBody] ItemDto itemDto)
        {
            ItemDto item = _itemService.UpdateItem(id, itemDto);
            if (item == null)
            {
                return BadRequest("Error occured while updating an item!");
            }
            return Ok(item);

        }

        [HttpGet("all")]
        [Authorize(Roles = "buyer")]
        public IActionResult GetAll()
        {
            List<ItemDto> items = new List<ItemDto>();
            items = _itemService.GetAll();
            if (items == null)
            {
                return BadRequest("Error occurred while getting all items!");
            }
            return Ok(items);
        }
    }
}
 