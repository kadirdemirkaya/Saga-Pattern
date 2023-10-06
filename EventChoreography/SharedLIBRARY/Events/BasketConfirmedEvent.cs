using SharedLIBRARY.Enums;
using SharedLIBRARY.Message;

namespace SharedLIBRARY.Events
{
    public class BasketConfirmedEvent
    {
        public int BasketId { get; set; }
        public int CustomerId { get; set; }

        public AddressMessage AddressMessage { get; set; }
        public PaymentMessage PaymentMessage { get; set; }
        public List<BasketItemMessage> BasketItemMessages { get; set; }
    }
}
