using MassTransit;

namespace SharedLIBRARY.Events
{
    public class StockNotEnoughtEvent : CorrelatedBy<Guid>
    {
        public int OrderId { get; set; }
        public int BasketId { get; set; }

        public Guid CorrelationId { get; }

        public StockNotEnoughtEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public StockNotEnoughtEvent()
        {
            
        }
    }
}
