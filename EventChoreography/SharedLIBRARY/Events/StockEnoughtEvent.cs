using SharedLIBRARY.Message;

namespace SharedLIBRARY.Events
{
    public class StockEnoughtEvent
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }
        public List<int> StockId { get; set; }

        public int BasketId { get; set; }

        public List<BasketItemMessage> BasketItemMessages { get; set; }
        public PaymentMessage PaymentMessage { get; set; }
    }
}
