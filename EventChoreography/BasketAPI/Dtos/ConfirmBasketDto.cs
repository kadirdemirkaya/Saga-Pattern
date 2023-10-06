using SharedLIBRARY.Enums;

namespace BasketAPI.Dtos
{
    public class ConfirmBasketDto
    {
        public int BasketId { get; set; }
        public int CustomerId { get; set; }

        public AddressDto AddressDto { get; set; }
        public PaymentDto PaymentDto { get; set; }
        public List<BasketItemDto> BasketItemDtos { get; set; }
    }
}
