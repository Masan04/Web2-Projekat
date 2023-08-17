using AutoMapper;
using Projekat.Dto;
using Projekat.Models;

namespace Projekat.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserRegisterDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
            CreateMap<Verification, VerificationDto>().ReverseMap(); 
            CreateMap<Item, ItemDto> ().ReverseMap(); 
            CreateMap<ItemsInsideOrder, ItemsInsideOrderDto>().ReverseMap();

        }
    }
}
