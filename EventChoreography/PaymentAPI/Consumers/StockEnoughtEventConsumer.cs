using MassTransit;
using PaymentAPI.Data;
using PaymentAPI.Model;
using SharedLIBRARY.Events;
using SharedLIBRARY.Repository.Generic;

namespace PaymentAPI.Consumers
{
    public class StockEnoughtEventConsumer : IConsumer<StockEnoughtEvent>
    {
        private readonly PaymentDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepository<Payment> _paymentRepository;

        public StockEnoughtEventConsumer( IPublishEndpoint publishEndpoint, PaymentDbContext dbContext)
        {
            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
            _paymentRepository = new Repository<Payment>(dbContext);
        }

        public async Task Consume(ConsumeContext<StockEnoughtEvent> context)
        {
            float amount = 0;
            var existCustomer = await _paymentRepository.GetFilter(p => p.CustomerId == context.Message.CustomerId);
            if (existCustomer is not null)
            {
                foreach (var basketItem in context.Message.BasketItemMessages)
                {
                    amount += (float)(basketItem.Count * (float)basketItem.Price);
                }
                if(existCustomer.Balance > amount)
                {
                    existCustomer.Balance -= amount;
                    existCustomer.Status = SharedLIBRARY.Enums.PaymentStatus.Completed;
                    await _paymentRepository.SaveChangesAsync();

                    var paymentCompletedEvent = new PaymentCompletedEvent
                    {
                        BasketId = context.Message.BasketId,
                        OrderId = context.Message.OrderId,
                        Status = SharedLIBRARY.Enums.PaymentStatus.Completed
                    };

                    await _publishEndpoint.Publish(paymentCompletedEvent);
                }
                else
                {
                    existCustomer.Status = SharedLIBRARY.Enums.PaymentStatus.Fail;
                    var paymentNotCompletedEvent = new PaymentNotCompletedEvent
                    {
                        Status = SharedLIBRARY.Enums.PaymentStatus.Fail,
                        BasketId = context.Message.BasketId,
                        OrderId = context.Message.OrderId,
                        StockId = context.Message.StockId,
                        Count = context.Message.BasketItemMessages.FirstOrDefault(bi => bi.BasketId == context.Message.BasketId).Count
                    };
                    await _publishEndpoint.Publish(paymentNotCompletedEvent);
                }
            }
            else
            {
                var paymentNotCompletedEvent = new PaymentNotCompletedEvent
                {
                    Status = SharedLIBRARY.Enums.PaymentStatus.Fail,
                    BasketId = context.Message.BasketId,
                    OrderId = context.Message.OrderId,
                    StockId = context.Message.StockId,
                    Count = default // !
                };
                await _publishEndpoint.Publish(paymentNotCompletedEvent);
            }
        }
    }
}
