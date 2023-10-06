using SharedLIBRARY.Enums;
using SharedLIBRARY.Models.BaseModel;

namespace BasketAPI.Models
{
    public class Basket : EntityBase
    {
        public BasketStatus Status { get; set; }
        public int CustomerId { get; set; }

        public ICollection<BasketItem> BasketItems { get; set; } = new List<BasketItem>();



        public static Basket Create(int customerId, BasketStatus status, string errorMessage)
        {
            return new Basket
            {
                CustomerId = customerId,
                CreatedDate = DateTime.Now,
                ErrorMessage = errorMessage,
                Status = status
            };
        }

        public void AddBasketItem(int productId, int basketId, decimal price, int count, BasketStatus status)
        {
            BasketItems.Add(new BasketItem(productId, basketId, price, count, status));
        }
    }
}
