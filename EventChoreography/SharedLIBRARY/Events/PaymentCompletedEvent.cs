using SharedLIBRARY.Enums;

namespace SharedLIBRARY.Events
{
    public class PaymentCompletedEvent
    {
        public int BasketId { get; set; }
        public int OrderId { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
