using SharedLIBRARY.Enums;

namespace BasketAPI.Dtos
{
    public class BasketItemDto
    {
        public int ProductId { get; set; }
        public int BasketId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
