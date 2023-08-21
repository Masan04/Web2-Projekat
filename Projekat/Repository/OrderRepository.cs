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

        public void AddItemsInsideOrder(ItemsInsideOrder itemsInsideRrder)
        {
            _dataContext.ItemsInsideOrders.Add(itemsInsideRrder);
            _dataContext.SaveChanges();

        }
    }
}
