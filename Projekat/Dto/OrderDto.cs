using Projekat.Models;

namespace Projekat.Dto
{
    public class OrderDto
    {
        public long Id { get; set; }
        public double Price { get; set; }
        public string Comment { get; set; }
        public string Address { get; set; }
        public OrderStatus Status { get; set; }
        public long SellerId { get; set; }
        public long BuyerId { get; set; }
        public string OrderTime { get; set; }
        public string OrderArriving { get; set; }
        public List<ItemAmount> itemAmounts { get; set; }
 
    }

    public class ItemAmount
    {
        public long itemId { get; set; }
        public int amount { get; set; }
    }
}
