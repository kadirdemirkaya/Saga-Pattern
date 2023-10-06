using MassTransit;
using SharedLIBRARY.Events;
using SharedLIBRARY.Message;
using StateMachineWorkerService.Models;

namespace StateMachineWorkerService.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<BasketCreatedEvent> BasketCreatedEventProp { get; set; }
        public State BasketCreated { get; private set; }

        public Event<BasketConfirmedEvent> BasketConfirmEventProp { get; set; }
        public State BasketConfirm { get; private set; }

        public Event<OrderCreatedEvent> OrderCreatedEventProp { get; set; }
        public State OrderCreated { get; private set; }

        public Event<StockEnoughtEvent> StockEnoughtEventProp { get; set; }
        public State StockEnought { get; private set; }

        public Event<PaymentCompletedEvent> PaymentCompletedEventProp { get; set; }
        public State PaymentCompleted { get; private set; }

        public Event<PaymentProcessSuccesfullyEvent> PaymentProcessSuccesfullyEventProp { get; set; }
        public State PaymentProcessSuccesfully { get; private set; }

        public Event<BasketCancelEvent> BasketCancelEventProp { get; set; }
        public State BasketCancel { get; private set; }

        public Event<StockNotEnoughtEvent> StockNotEnoughtEventProp { get; set; }
        public State StockNotEnought { get; private set; }

        public Event<PaymentNotCompletedEvent> PaymentNotCompletedEventProp { get; set; }
        public State PaymentNotCompleted { get; private set; }

        public OrderStateMachine()
        {
            #region
            InstanceState(i => i.CurrentState);

            Event(() => BasketCreatedEventProp, b => b.CorrelateBy<int>(c => c.BasketId, c => c.Message.BasketId).SelectId(context => Guid.NewGuid()));
            Event(() => BasketConfirmEventProp, o => o.CorrelateById(c => c.Message.CorrelationId));
            Event(() => OrderCreatedEventProp, o => o.CorrelateById(c => c.Message.CorrelationId));
            Event(() => StockEnoughtEventProp, o => o.CorrelateById(c => c.Message.CorrelationId));
            Event(() => PaymentCompletedEventProp, o => o.CorrelateById(c => c.Message.CorrelationId));
            Event(() => PaymentProcessSuccesfullyEventProp, o => o.CorrelateById(c => c.Message.CorrelationId));

            Event(() => BasketCancelEventProp, o => o.CorrelateById(c => c.Message.CorrelationId));
            Event(() => StockNotEnoughtEventProp, o => o.CorrelateById(c => c.Message.CorrelationId));
            Event(() => PaymentNotCompletedEventProp, o => o.CorrelateById(c => c.Message.CorrelationId));

            Initially(When(BasketCreatedEventProp).Then(context =>
            {
                context.Instance.BasketId = context.Data.BasketId;
                context.Instance.CustomerId = context.Data.CustomerId;
                context.Instance.City = context.Data.AddressMessage.City;
                context.Instance.Country = context.Data.AddressMessage.Country;
                context.Instance.FullAddress = context.Data.AddressMessage.FullAddress;
                context.Instance.Neighbourhood = context.Data.AddressMessage.Neighbourhood;
                context.Instance.CardholderLastname = context.Data.PaymentMessage.CardholderLastname;
                context.Instance.CardholderName = context.Data.PaymentMessage.CardholderName;
                context.Instance.CardNumber = context.Data.PaymentMessage.CardNumber;
                context.Instance.CVV = context.Data.PaymentMessage.CVV;
                context.Instance.Expiration = context.Data.PaymentMessage.Expiration;
            }).Then(context =>
            {
                Console.WriteLine("BasketConfirmEventProp Then : " + context.Instance ?? DateTime.Now.ToShortDateString());
            }).Publish(context => new BasketConfirmedEvent(context.Instance.CorrelationId)
            {
                AddressMessage = new AddressMessage()
                {
                    City = context.Instance.City,
                    Neighbourhood = context.Instance.Neighbourhood,
                    Country = context.Instance.Country,
                    FullAddress = context.Instance.FullAddress
                },
                BasketId = context.Instance.BasketId,
                BasketItemMessages = context.Data.BasketItemMessages,
                CustomerId = context.Instance.CustomerId,
                PaymentMessage = new PaymentMessage()
                {
                    Expiration = context.Instance.Expiration,
                    CVV = context.Instance.CVV,
                    CardNumber = context.Instance.CardNumber,
                    CardholderName = context.Instance.CardholderName,
                    CardholderLastname = context.Instance.CardholderLastname
                }
            }).TransitionTo(BasketConfirm)
            .Then(context =>
            {
                Console.WriteLine("BasketConfirmEventProp Datas : " + context.Instance ?? DateTime.Now.ToShortDateString());
            }));
            #endregion



            During(BasketConfirm, When(OrderCreatedEventProp).TransitionTo(StockEnought)
            .Publish(context => new OrderCreatedEvent(context.Instance.CorrelationId)
            {
                BasketId = context.Instance.BasketId,
                CustomerId = context.Instance.CustomerId,
                BasketItemMessages = context.Data.BasketItemMessages,
                ProductId = context.Data.ProductId,
                OrderId = context.Data.OrderId,
                PaymentMessage = new PaymentMessage()
                {
                    Expiration = context.Instance.Expiration,
                    CVV = context.Instance.CVV,
                    CardNumber = context.Instance.CardNumber,
                    CardholderName = context.Instance.CardholderName,
                    CardholderLastname = context.Instance.CardholderLastname
                }
            }).Then(context =>
            {
                Console.WriteLine("OrderCreatedEvent Then : "+ context.Instance ?? DateTime.Now.ToShortDateString());
            }), When(BasketCancelEventProp).TransitionTo(BasketCancel)
            .Publish(context => new BasketCancelEvent(context.Instance.CorrelationId)
            {
                BasketId = context.Instance.BasketId
            }).Then(context =>
            {
                Console.WriteLine("BasketCancelEvent Error : "+ context.Instance ?? DateTime.Now.ToShortDateString());
            }));




            During(StockEnought, When(StockEnoughtEventProp).TransitionTo(PaymentCompleted)
                .Publish(context => new StockEnoughtEvent(context.Instance.CorrelationId)
                {
                    OrderId = context.Data.OrderId,
                    StockId = context.Data.StockId,
                    BasketId = context.Data.BasketId,
                    CustomerId = context.Data.CustomerId,
                    PaymentMessage = new PaymentMessage()
                    {
                        Expiration = context.Data.PaymentMessage.Expiration,
                        CVV = context.Data.PaymentMessage.CVV,
                        CardNumber = context.Data.PaymentMessage.CardNumber,
                        CardholderName = context.Data.PaymentMessage.CardholderName,
                        CardholderLastname = context.Data.PaymentMessage.CardholderLastname
                    },
                    BasketItemMessages = context.Data.BasketItemMessages
                }).Then(context =>
                {
                    Console.WriteLine("StockEnoughtEvent Then : " + context.Instance ?? DateTime.Now.ToShortDateString());
                }), When(StockNotEnoughtEventProp).TransitionTo(StockNotEnought).
                Publish(context => new StockNotEnoughtEvent(context.Instance.CorrelationId)
                {
                    BasketId = context.Instance.BasketId,
                    OrderId = context.Data.OrderId
                }).Then(context =>
                {
                    Console.WriteLine("StockNotEnoughtEvent Error : " + context.Instance ?? DateTime.Now.ToShortDateString());
                }));



            During(PaymentCompleted, When(PaymentCompletedEventProp).TransitionTo(PaymentProcessSuccesfully)
            .Publish(context => new PaymentCompletedEvent(context.Instance.CorrelationId)
            {
                BasketId = context.Instance.BasketId,
                OrderId = context.Data.OrderId,
                Status = context.Data.Status
            }).Then(context =>
            {
                Console.WriteLine("PaymentCompletedEvent Then : " + context.Instance ?? DateTime.Now.ToShortDateString());
            }).Finalize(), When(PaymentNotCompletedEventProp)
            .Publish(context => new PaymentNotCompletedEvent(context.Instance.CorrelationId)
            {
                BasketId = context.Instance.BasketId,
                OrderId = context.Data.OrderId,
                Status = context.Data.Status,
                StockAndCounts = context.Data.StockAndCounts,
                StockId = context.Data.StockId
            }).TransitionTo(PaymentNotCompleted)
            .Then(context =>
            {
                Console.WriteLine("PaymentNotCompletedEvent Error : " + context.Instance ?? DateTime.Now.ToShortDateString());
            }));

            SetCompletedWhenFinalized();
        }
    }
}
