using MassTransit;
using SharedLIBRARY.Message;

namespace SharedLIBRARY.Events
{
    public class StockEnoughtEvent : CorrelatedBy<Guid>
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }
        public List<int> StockId { get; set; }

        public int BasketId { get; set; }

        public List<BasketItemMessage> BasketItemMessages { get; set; }
        public PaymentMessage PaymentMessage { get; set; }

        public Guid CorrelationId { get; }

        public StockEnoughtEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public StockEnoughtEvent()
        {

        }
    }
}
