using AutoMapper;
using BasketAPI.Dtos;
using BasketAPI.Models;
using SharedLIBRARY.Message;

namespace BasketAPI.Mapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<BasketItem, BasketItemMessage>().ReverseMap();
            CreateMap<BasketItemMessage, BasketItemDto>().ReverseMap();
            CreateMap<PaymentDto, PaymentMessage>().ReverseMap();
            CreateMap<AddressDto, AddressMessage>().ReverseMap();
        }
    }
}
