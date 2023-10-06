namespace SharedLIBRARY.Message
{
    public class BasketItemMessage
    {
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public int BasketId { get; set; }

        public BasketItemMessage()
        {
            
        }

        public BasketItemMessage(int productId, int count, decimal price, int basketId)
        {
            Price = price;
            BasketId = basketId;
            ProductId = productId;
            Count = count;
        }
    }
}
