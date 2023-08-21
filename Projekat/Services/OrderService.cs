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
            ItemOrderDto itemOrderDto = new ItemOrderDto();
            itemOrderDto.ItemId = itemId;
            itemOrderDto.OrderId = order.Id;
            itemOrderDto.Amount = amount;

            ItemOrder itemOrder = _mapper.Map<ItemOrder>(itemOrderDto);

            _orderRepository.AddItemsInsideOrder(itemOrder);
        }

        public OrderDto DeleteOrder(long id)
        {
            try
            {
                List<ItemOrder> itemsInsideOrder = _orderRepository.FindItemOrderByOrderId(id);
                foreach (var orderItem in itemsInsideOrder)
                {
                   
                    orderItem.Item.Amount += orderItem.Amount;

                    _orderRepository.SaveChanges();
                }

                Order order = _orderRepository.FindOrderById(id);
                order.Status = OrderStatus.CANCELED;
                _orderRepository.SaveChanges();

                return _mapper.Map<OrderDto>(order);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<OrderDto> GetAll()
        {
            try
            {
                return _mapper.Map<List<OrderDto>>(_orderRepository.GetAllOrders());

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<OrderDto> GetNewOrdersBySellerId(long sellerId)
        {
            try
            {
                List<Order> orders = _orderRepository.GetNewOrdersBySeller(sellerId);
                List<OrderDto> orderDtos = new List<OrderDto>();

                foreach (var order in orders)
                {
                    if (order.Status == OrderStatus.IN_PROCESS)
                    {
                        int temp = -1;
                        Tuple<int, int> rezultat = CalculateTime(order.OrderTime, order.OrderArriving, temp);

                        if (rezultat.Item2 == 1)
                        {
                            order.Status = OrderStatus.DONE;
                            _orderRepository.SaveChanges();
                        }
                        else
                            orderDtos.Add(_mapper.Map<OrderDto>(order));
                    }
                }

                return orderDtos;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<OrderCancelCheckDto> GetOrdersByBuyerId(long buyerId)
        {
            try
            {
                List<Order> orders = _orderRepository.GetOrdersByBuyer(buyerId);
                List<int> otkazi = new List<int>();
                int otkaz = 0;
                foreach (var order in orders)
                {
                    if (order.Status == OrderStatus.IN_PROCESS)
                    {
                        Tuple<int, int> rezultat = CalculateTime(order.OrderTime, order.OrderArriving, otkaz);

                        if (rezultat.Item1 == 1)
                            otkazi.Add(1);
                        else
                            otkazi.Add(0);
                        otkaz = 0;

                        if (rezultat.Item2 == 1)
                        {
                            order.Status = OrderStatus.DONE;
                            _orderRepository.SaveChanges();
                        }
                    }
                    else
                        otkazi.Add(0);
                }
                List<OrderCancelCheckDto> orderCancelCheckDtos = _mapper.Map<List<OrderCancelCheckDto>>(orders);
                int counter = 0;
                foreach (var order in orderCancelCheckDtos)
                {
                    order.Cancel = otkazi[counter];
                    counter++;
                }

                return orderCancelCheckDtos;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<OrderDto> GetPastOrdersBySellerId(long sellerId)
        {
            try
            {
                List<Order> orders = _orderRepository.GetPastOrdersBySeller(sellerId);
                return _mapper.Map<List<OrderDto>>(orders);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static Tuple<int, int> CalculateTime(string orderTime, string orderArriving, int otkaz)
        {
            DateTime orderDateTime = DateTime.ParseExact(orderTime, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
            DateTime targetTime = DateTime.ParseExact(orderArriving, "M/d/yyyy, h:mm:ss tt", CultureInfo.InvariantCulture);
            DateTime currentTime = DateTime.Now;
            int delivered = 0;

            if (targetTime < currentTime)
            {
                delivered = 1;
            }
            else if (currentTime < orderDateTime.AddHours(1))
            {
                otkaz = 1;
            }

            Tuple<int, int> rezultat = new Tuple<int, int>(otkaz, delivered);
            return rezultat;
        }

        public int GetNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(60, 181);
            return randomNumber;
        }
    }
}
