using MassTransit;

namespace SharedLIBRARY.Events
{
    public class BasketCancelEvent : CorrelatedBy<Guid>
    {
        public int BasketId { get; set; }
        public string ErrorMessage { get; set; }

        public Guid CorrelationId { get; }

        public BasketCancelEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public BasketCancelEvent()
        {
            
        }
    }
}
