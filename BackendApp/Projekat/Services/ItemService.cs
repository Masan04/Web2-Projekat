using AutoMapper;
using Projekat.Dto;
using Projekat.Interfaces;
using Projekat.Models;
using Projekat.Repository;

namespace Projekat.Services
{
    public class ItemService : IItemService
    {
        private readonly IMapper _mapper;
        private readonly ItemRepository _itemRepository;

        public ItemService(IMapper mapper, ItemRepository itemRepository)
        {
            _mapper = mapper;
            _itemRepository = itemRepository;
        }

        public ItemDto CreateItem(ItemDto itemCreate)
        {
            try
            {
                Item item = _mapper.Map<Item>(itemCreate);
                _itemRepository.AddItem(item);

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
                Item item = _itemRepository.FindItemById(id);
                _itemRepository.RemoveItem(item);

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
                return _mapper.Map<List<ItemDto>>(_itemRepository.GetAllItems());
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ItemDto GetItemById(long id)
        {
            return _mapper.Map<ItemDto>(_itemRepository.FindItemById(id));
        }



        public List<ItemDto> GetItemsByOrderId(long orderId)
        {
            try
            {
                List<ItemOrderDto> itemsInsideOrderDto = _mapper.Map<List<ItemOrderDto>>(_itemRepository.GetItemOrderByOrderId(orderId));
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
                return _mapper.Map<List<ItemDto>>(_itemRepository.GetItemsBySellerId(sellerId));
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
                Item item =_itemRepository.UpdateItem(id, itemDto);

                return _mapper.Map<ItemDto>(item);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ItemDto UpdateItemAfterOrder(long id, int amount)
        {
            Item itemDb = _itemRepository.FindItemById(id);
            itemDb.Amount -= amount;
            _itemRepository.SaveChanges();

            return _mapper.Map<ItemDto>(itemDb);
        }
    }
}
