using MassTransit;

namespace SharedLIBRARY.Events
{
    public class PaymentProcessSuccesfullyEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; }

        public PaymentProcessSuccesfullyEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public PaymentProcessSuccesfullyEvent()
        {
            
        }
    }
}
