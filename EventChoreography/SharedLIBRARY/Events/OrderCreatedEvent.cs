using SharedLIBRARY.Message;

namespace SharedLIBRARY.Events
{
    public class OrderCreatedEvent
    {
        public int BasketId { get; set; }
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public List<int> ProductId { get; set; }
        public List<BasketItemMessage> BasketItemMessages { get; set; }
        public PaymentMessage PaymentMessage { get; set; }
    }
}