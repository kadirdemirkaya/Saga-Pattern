using AutoMapper;
using MassTransit;
using OrderAPI.Data;
using OrderAPI.Models;
using SharedLIBRARY.Events;
using SharedLIBRARY.Repository.Generic;

namespace OrderAPI.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly OrderDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepository<Order> _orderRepository;

        public PaymentCompletedEventConsumer(IPublishEndpoint publishEndpoint, OrderDbContext dbContext)
        {

            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
            _orderRepository = new Repository<Order>(dbContext);
        }
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _orderRepository.GetByIdAsync(context.Message.OrderId);
            order.Status = SharedLIBRARY.Enums.OrderStatus.Completed;
            await _orderRepository.SaveChangesAsync();
        }
    }
}
