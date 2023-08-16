using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekat.Models;

namespace Projekat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly DataContext _context;


        private static List<Item> items = new List<Item>
        {
            new Item { Id = 1, Name = "Kazein", Amonunt = 1, Description="suplement", Picture="Neka slika", Price= 10000, SellerId = 1}

        };

        [HttpGet]
        public async Task<ActionResult<List<Item>>> Get()
        {
            return Ok(await _context.Items.ToListAsync());
        }

        public ItemController(DataContext context)
        {
            _context = context;
        }

    }
}
 