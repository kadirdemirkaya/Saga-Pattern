using AutoMapper;
using PaymentAPI.Dtos;
using PaymentAPI.Model;
using SharedLIBRARY.Message;

namespace PaymentAPI.Mapper
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentMessage>();
            CreateMap<Payment, PaymentDto>();
        }
    }
}
