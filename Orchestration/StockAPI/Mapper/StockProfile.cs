using AutoMapper;
using StockAPI.Dtos;
using StockAPI.Model;

namespace StockAPI.Mapper
{
    public class StockProfile : Profile
    {
        public StockProfile()
        {
            CreateMap<Stock, StockDto>();
        }
    }
}
