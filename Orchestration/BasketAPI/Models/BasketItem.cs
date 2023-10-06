using SharedLIBRARY.Enums;
using SharedLIBRARY.Models.BaseModel;

namespace BasketAPI.Models
{
    public class BasketItem : EntityBase
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public BasketStatus Status { get; set; }


        public BasketItem(int productId,int basketId, decimal price, int count, BasketStatus status)
        {
            ProductId = productId;
            BasketId = basketId;
            Price = price;
            Count = count;
            Status = status;
        }

        public int BasketId { get; set; }
        public Basket Basket { get; set; }
    }
}
