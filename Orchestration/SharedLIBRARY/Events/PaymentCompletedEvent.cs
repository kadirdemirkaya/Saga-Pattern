using MassTransit;
using SharedLIBRARY.Enums;

namespace SharedLIBRARY.Events
{
    public class PaymentCompletedEvent : CorrelatedBy<Guid>
    {
        public int BasketId { get; set; }
        public int OrderId { get; set; }
        public PaymentStatus Status { get; set; }

        public Guid CorrelationId { get; }

        public PaymentCompletedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public PaymentCompletedEvent()
        {

        }
    }
}
