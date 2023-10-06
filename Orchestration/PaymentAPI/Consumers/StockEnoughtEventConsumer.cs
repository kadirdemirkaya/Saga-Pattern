using MassTransit;
using PaymentAPI.Data;
using PaymentAPI.Model;
using SharedLIBRARY.Events;
using SharedLIBRARY.Message;
using SharedLIBRARY.Repository.Generic;

namespace PaymentAPI.Consumers
{
    public class StockEnoughtEventConsumer : IConsumer<StockEnoughtEvent>
    {
        private readonly PaymentDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepository<Payment> _paymentRepository;

        public StockEnoughtEventConsumer(IPublishEndpoint publishEndpoint, PaymentDbContext dbContext)
        {
            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
            _paymentRepository = new Repository<Payment>(dbContext);
        }

        public async Task Consume(ConsumeContext<StockEnoughtEvent> context)
        {
            float amount = 0;
            var existCustomer = await _paymentRepository.GetFilter(p => p.CustomerId == context.Message.CustomerId && p.CVV == context.Message.PaymentMessage.CVV && p.CardNumber == context.Message.PaymentMessage.CardNumber);
            if (existCustomer is not null)
            {
                foreach (var basketItem in context.Message.BasketItemMessages)
                {
                    amount += (float)(basketItem.Count * (float)basketItem.Price);
                }
                if (existCustomer.Balance > amount)
                {
                    existCustomer.Balance -= amount;
                    existCustomer.Status = SharedLIBRARY.Enums.PaymentStatus.Completed;
                    await _paymentRepository.SaveChangesAsync();

                    var paymentCompletedEvent = new PaymentCompletedEvent(context.Message.CorrelationId)
                    {
                        OrderId = context.Message.OrderId,
                        Status = SharedLIBRARY.Enums.PaymentStatus.Completed,
                        BasketId = context.Message.BasketId
                    };

                    await _publishEndpoint.Publish(paymentCompletedEvent);
                }
                else
                {
                    existCustomer.Status = SharedLIBRARY.Enums.PaymentStatus.Fail;
                    var paymentNotCompletedEvent = new PaymentNotCompletedEvent(context.Message.CorrelationId)
                    {
                        Status = SharedLIBRARY.Enums.PaymentStatus.Fail,
                        OrderId = context.Message.OrderId,
                        StockId = context.Message.StockId,
                        StockAndCounts = StockAndCountList(context.Message.BasketItemMessages),
                        BasketId = context.Message.BasketId,
                    };
                    await _publishEndpoint.Publish(paymentNotCompletedEvent);
                }
            }
            else
            {
                var paymentNotCompletedEvent = new PaymentNotCompletedEvent(context.Message.CorrelationId)
                {
                    Status = SharedLIBRARY.Enums.PaymentStatus.Fail,
                    OrderId = context.Message.OrderId,
                    StockId = context.Message.StockId,
                    StockAndCounts = StockAndCountList(context.Message.BasketItemMessages),
                    BasketId = context.Message.BasketId
                };
                await _publishEndpoint.Publish(paymentNotCompletedEvent);
            }
        }

        private Dictionary<int, int> StockAndCountList(List<BasketItemMessage> basketItemMessages)
        {
            Dictionary<int, int> sc = new();
            foreach (var basketItem in basketItemMessages)
            {
                sc.Add(basketItem.BasketId, basketItem.Count);
            }
            return sc;
        }
    }
}
