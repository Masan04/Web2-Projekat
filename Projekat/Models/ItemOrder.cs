namespace Projekat.Models
{
    public class ItemOrder
    {
        public long Id { get; set; }
        public Item Item { get; set; }
        public Order Order { get; set; }
        public int Amount { get; set; }
    }
}
