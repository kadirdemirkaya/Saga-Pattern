using AutoMapper;
using OrderAPI.Dtos;
using OrderAPI.Models;
using SharedLIBRARY.Message;

namespace OrderAPI.Mapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Address, AddressMessage>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
