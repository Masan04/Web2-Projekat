namespace Projekat.Models
{
    public class ItemOrder
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public long OrderId { get; set; }
        public int Amount { get; set; }
    }
}
