using MassTransit;
using SharedLIBRARY.Message;

namespace SharedLIBRARY.Events
{
    public class BasketCreatedEvent : CorrelatedBy<Guid>
    {
        public int BasketId { get; set; }
        public int CustomerId { get; set; }

        public AddressMessage AddressMessage { get; set; }
        public PaymentMessage PaymentMessage { get; set; }
        public List<BasketItemMessage> BasketItemMessages { get; set; }

        public Guid CorrelationId { get; }

        public BasketCreatedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public BasketCreatedEvent()
        {

        }
    }

    /*
     
      public class BasketConfirmedEvent : CorrelatedBy<Guid>
    {
        public int BasketId { get; set; }
        public int CustomerId { get; set; }

        public AddressMessage AddressMessage { get; set; }
        public PaymentMessage PaymentMessage { get; set; }
        public List<BasketItemMessage> BasketItemMessages { get; set; }

        public Guid CorrelationId { get; }

        public BasketConfirmedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public BasketConfirmedEvent()
        {

        }
    }
     
     */
}
