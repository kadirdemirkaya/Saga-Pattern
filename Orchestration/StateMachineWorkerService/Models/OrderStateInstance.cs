using MassTransit;
using SharedLIBRARY.Enums;

namespace StateMachineWorkerService.Models
{
    public class OrderStateInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        public int BasketId { get; set; }

        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CardholderName { get; set; }
        public string CardholderLastname { get; set; }
        public string CVV { get; set; }

        public string Country { get; set; }
        public string City { get; set; }
        public string Neighbourhood { get; set; }
        public string FullAddress { get; set; }

        public PaymentStatus Status { get; set; }
        public int CustomerId { get; set; }
    }
}
