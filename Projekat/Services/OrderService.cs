using AutoMapper;
using Projekat.Data;
using Projekat.Dto;
using Projekat.Interfaces;
using Projekat.Models;
using Projekat.Repository;
using System.Globalization;

namespace Projekat.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly OrderRepository _orderRepository;
        private readonly IItemService _itemService;
        

        public OrderService(IMapper mapper, OrderRepository orderRepository, IItemService itemService)
        {
            _mapper = mapper;
            _orderRepository = orderRepository;
            _itemService = itemService;
        }

        public OrderDto CreateOrder(OrderDto orderDto)
        {
            try
            {
                Order order = _mapper.Map<Order>(orderDto);
                order.Status = OrderStatus.IN_PROCESS;
                DateTime orderTime = DateTime.ParseExact(order.OrderTime, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
                int rng = GetNumber();
                DateTime targetTime = orderTime.AddMinutes(rng);
                order.OrderArriving = targetTime.ToString("M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);

                _orderRepository.AddOrder(order);

                
                foreach (var itemAmount in orderDto.itemAmounts)
                {
                    updateItemAmountInsideOrder(itemAmount, order);
                 
                }
               

                return _mapper.Map<OrderDto>(order);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void updateItemAmountInsideOrder(ItemAmount itemAmount, Order order)
        {
            long itemId = itemAmount.itemId;
            int amount = itemAmount.amount;
            _itemService.UpdateItemAfterOrder(itemId, amount);
            ItemsInsideOrderDto itemOrderDto = new ItemsInsideOrderDto();
            itemOrderDto.ItemId = itemId;
            itemOrderDto.OrderId = order.Id;
            itemOrderDto.Amount = amount;

            ItemsInsideOrder itemOrder = _mapper.Map<ItemsInsideOrder>(itemOrderDto);

            _orderRepository.AddItemsInsideOrder(itemOrder);
        }

        public OrderDto DeleteOrder(long id)
        {
            throw new NotImplementedException();
        }

        public List<OrderDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<OrderDto> GetNewOrdersBySellerId(long sellerId)
        {
            throw new NotImplementedException();
        }

        public List<OrderCancelCheckDto> GetOrdersByBuyerId(long buyerId)
        {
            throw new NotImplementedException();
        }

        public List<OrderDto> GetPastOrdersBySellerId(long sellerId)
        {
            throw new NotImplementedException();
        }
        public int GetNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(60, 181);
            return randomNumber;
        }
    }
}
