using SharedLIBRARY.Enums;

namespace SharedLIBRARY.Events
{
    public class PaymentNotCompletedEvent
    {
        public PaymentStatus Status { get; set; }
        public int BasketId { get; set; }
        public int OrderId { get; set; }
        public int Count { get; set; }
        public List<int> StockId { get; set; }
    }
}
