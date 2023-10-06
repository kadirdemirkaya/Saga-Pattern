using MassTransit;
using SharedLIBRARY.Message;

namespace SharedLIBRARY.Events
{
    public class OrderCreatedEvent : CorrelatedBy<Guid>
    {
        public int OrderId { get; set; }
        public List<int> ProductId { get; set; }
        public int BasketId { get; set; }
        public int CustomerId { get; set; }

        public PaymentMessage PaymentMessage { get; set; }
        public List<BasketItemMessage> BasketItemMessages { get; set; }

        public Guid CorrelationId { get; }

        public OrderCreatedEvent()
        {

        }
        public OrderCreatedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}