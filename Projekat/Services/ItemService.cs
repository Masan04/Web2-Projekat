using AutoMapper;
using Projekat.Dto;
using Projekat.Interfaces;
using Projekat.Models;

namespace Projekat.Services
{
    public class ItemService : IItemService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;

        public ItemService(IMapper mapper, DataContext dataContext)
        {
            _mapper = mapper;
            _dataContext = dataContext;
        }

        public ItemDto CreateItem(ItemDto itemCreate)
        {
            try
            {
                Item item = _mapper.Map<Item>(itemCreate);
                _dataContext.Items.Add(item);
                _dataContext.SaveChanges();

                return _mapper.Map<ItemDto>(item);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool DeleteItem(long id)
        {
            try
            {
                Item item = _dataContext.Items.Find(id);
                _dataContext.Items.Remove(item);
                _dataContext.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<ItemDto> GetAll()
        {
            try
            {
                return _mapper.Map<List<ItemDto>>(_dataContext.Items.ToList());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ItemDto GetItemById(long id)
        {
            return _mapper.Map<ItemDto>(_dataContext.Items.First(x => x.Id == id));
        }



        public List<ItemDto> GetItemsByOrderId(long orderId)
        {
            try
            {
                List<ItemsInsideOrderDto> itemsInsideOrderDto = _mapper.Map<List<ItemsInsideOrderDto>>(_dataContext.ItemsInsideOrders.ToList().FindAll(i => i.OrderId == orderId));
                List<ItemDto> items = new List<ItemDto>();
                foreach (var item in itemsInsideOrderDto)
                {
                    ItemDto itemDb = GetItemById(item.ItemId);
                    itemDb.Amount = item.Amount;
                    items.Add(itemDb);
                }

                return items;
            } 
            
            catch(Exception)
            {
                return null;
            }
            
            
        }

        public List<ItemDto> GetItemsBySellerId(long sellerId)
        {
            try
            {
                return _mapper.Map<List<ItemDto>>(_dataContext.Items.ToList().FindAll(i => i.SellerId == sellerId));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ItemDto UpdateItem(long id, ItemDto itemDto)
        {
            try
            {
                Item newItem = _mapper.Map<Item>(itemDto);
                Item itemDb = _dataContext.Items.Find(id);

                itemDb.Name = newItem.Name;
                itemDb.Price = newItem.Price;
                itemDb.Picture = newItem.Picture;
                itemDb.Amount = newItem.Amount;
                itemDb.Description = newItem.Description;

                _dataContext.SaveChanges();

                return _mapper.Map<ItemDto>(itemDb);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ItemDto UpdateItemAfterOrder(long id, int amount)
        {
            Item itemDb = _dataContext.Items.Find(id);
            itemDb.Amount -= amount;
            _dataContext.SaveChanges();

            return _mapper.Map<ItemDto>(itemDb);
        }
    }
}
