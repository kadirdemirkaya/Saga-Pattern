using AutoMapper;
using MassTransit;
using OrderAPI.Data;
using OrderAPI.Models;
using SharedLIBRARY.Events;
using SharedLIBRARY.Repository.Generic;

namespace OrderAPI.Consumers
{
    public class PaymentNotCompletedEventConsumer : IConsumer<PaymentNotCompletedEvent>
    {
        private readonly OrderDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepository<Order> _orderRepository;

        public PaymentNotCompletedEventConsumer(IPublishEndpoint publishEndpoint, OrderDbContext dbContext)
        {
            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
            _orderRepository = new Repository<Order>(dbContext);
        }

        public async Task Consume(ConsumeContext<PaymentNotCompletedEvent> context)
        {
            var order = await _orderRepository.GetByIdAsync(context.Message.OrderId);
            order.Status = SharedLIBRARY.Enums.OrderStatus.Fail;
            order.ErrorMessage = "Payment Not Completed Event Error";
            await _orderRepository.SaveChangesAsync();
        }
    }
}
