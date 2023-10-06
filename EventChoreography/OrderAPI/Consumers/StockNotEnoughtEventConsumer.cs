using MassTransit;
using OrderAPI.Data;
using OrderAPI.Models;
using SharedLIBRARY.Events;
using SharedLIBRARY.Repository.Generic;

namespace OrderAPI.Consumers
{
    public class StockNotEnoughtEventConsumer : IConsumer<StockNotEnoughtEvent>
    {
        private readonly OrderDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepository<Order> _orderRepository;

        public StockNotEnoughtEventConsumer(IPublishEndpoint publishEndpoint, OrderDbContext dbContext)
        {
            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
            _orderRepository = new Repository<Order>(dbContext);
        }
        public async Task Consume(ConsumeContext<StockNotEnoughtEvent> context)
        {
            Order? order = await _orderRepository.GetByIdAsync(context.Message.OrderId);
            order.Status = SharedLIBRARY.Enums.OrderStatus.Fail;
            order.ErrorMessage = "Stock Not Enought Event Message";
            await _orderRepository.SaveChangesAsync();
        }
    }
}
