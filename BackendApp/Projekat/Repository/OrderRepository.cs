using Projekat.Models;

namespace Projekat.Repository
{
    public class OrderRepository
    {
        private readonly DataContext _dataContext;

        public OrderRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void AddOrder(Order order)
        {
            _dataContext.Orders.Add(order);
            _dataContext.SaveChanges();

        }

        public void AddItemsInsideOrder(ItemOrder itemsInsideRrder)
        {
            _dataContext.ItemsInsideOrders.Add(itemsInsideRrder);
            _dataContext.SaveChanges();

        }
        public List<ItemOrder>? FindItemOrderByOrderId(long id)
        {
            return _dataContext.ItemsInsideOrders.ToList().FindAll(x => x.Order.Id == id);
        }

        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }

        public Order? FindOrderById(long id)
        {
            return _dataContext.Orders.Find(id);
        }

        public List<Order> GetAllOrders()
        {
            return _dataContext.Orders.ToList();
        }

        public List<Order> GetPastOrdersBySeller(long sellerId)
        {
            return _dataContext.Orders.ToList().FindAll(x => x.SellerId == sellerId && x.Status == OrderStatus.DONE);
        }

        public List<Order> GetNewOrdersBySeller(long sellerId)
        {
            return _dataContext.Orders.ToList().FindAll(x => x.SellerId == sellerId && x.Status == OrderStatus.CANCELED);
        }

        public List<Order> GetOrdersByBuyer(long buyerId)
        {
            return _dataContext.Orders.ToList().FindAll(x => x.BuyerId == buyerId && x.Status == OrderStatus.CANCELED);
        }

    }
}
