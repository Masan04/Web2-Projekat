using Projekat.Data;
using Projekat.Dto;
using Projekat.Models;

namespace Projekat.Repository
{
    public class ItemRepository
    {
        private readonly DataContext _dataContext;

        public ItemRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void AddItem(Item item)
        {
            _dataContext.Items.Add(item);
            _dataContext.SaveChanges();

        }

        public Item? FindItemById(long id)
        {
            return _dataContext.Items.Find(id);
        }

        public void RemoveItem(Item item)
        {
            _dataContext.Items.Remove(item);
            _dataContext.SaveChanges();
        }
        public List<Item> GetAllItems()
        {
            return _dataContext.Items.ToList();
        }

        public List<ItemOrder> GetItemOrderByOrderId(long orderId)
        {
            return _dataContext.ItemsInsideOrders.ToList().FindAll(i => i.OrderId == orderId);
        }
        public List<Item> GetItemsBySellerId(long sellerId)
        {
            return _dataContext.Items.ToList().FindAll(i => i.SellerId == sellerId);
        }

        public void SaveChanges()
        {
           _dataContext.SaveChanges();
        }

        public Item UpdateItem(long id,ItemDto itemDto)
        {
            Item itemDb = _dataContext.Items.Find(id);

            itemDb.Name = itemDto.Name;
            itemDb.Price = itemDto.Price;
            itemDb.Picture = itemDto.Picture;
            itemDb.Amount = itemDto.Amount;
            itemDb.Description = itemDto.Description;
            _dataContext.SaveChanges();
            return itemDb;
        }
    }
}
