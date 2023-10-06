using MassTransit;
using SharedLIBRARY.Enums;

namespace SharedLIBRARY.Events
{
    public class PaymentNotCompletedEvent : CorrelatedBy<Guid>
    {
        public PaymentStatus Status { get; set; }
        public int BasketId { get; set; }
        public int OrderId { get; set; }
        public Dictionary<int,int> StockAndCounts { get; set; }
        public List<int> StockId { get; set; }

        public Guid CorrelationId { get; }

        public PaymentNotCompletedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public PaymentNotCompletedEvent()
        {
            
        }
    }
}
