namespace Projekat.Dto
{
    public class ItemOrderDto
    {
        public long Id { get; set; }
        public long ItemId { get; set; }
        public long OrderId { get; set; }
        public int Amount { get; set; }
    }
}
