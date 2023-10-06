using AutoMapper;
using MassTransit;
using OrderAPI.Data;
using OrderAPI.Models;
using SharedLIBRARY.Enums;
using SharedLIBRARY.Events;
using SharedLIBRARY.Repository.Generic;

namespace OrderAPI.Consumers
{
    public class BasketConfirmedEventConsumer : IConsumer<BasketConfirmedEvent>
    {
        private readonly OrderDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;

        public BasketConfirmedEventConsumer(ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint, OrderDbContext dbContext, IMapper mapper)
        {
            _publishEndpoint = publishEndpoint;
            _orderRepository = new Repository<Order>(dbContext);
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<BasketConfirmedEvent> context)
        {
            var orderData = await _orderRepository.GetFilter(o => o.BasketId == context.Message.BasketId);
            if (orderData is not null)
            {
                var basketcancelevent = new BasketCancelEvent
                {
                    BasketId = orderData.BasketId,
                    ErrorMessage = "Basket already exists"
                };
                await _publishEndpoint.Publish(basketcancelevent);
            }
            else
            {
                Order order = new Order()
                {
                    Status = OrderStatus.Uncertain,
                    Address = _mapper.Map<Address>(context.Message.AddressMessage),
                    BasketId = context.Message.BasketId,
                    ErrorMessage = string.Empty,
                };
                await _orderRepository.AddAsync(order);
                await _orderRepository.SaveChangesAsync();

                var orderCreatedEvent = new OrderCreatedEvent(context.Message.CorrelationId)
                {
                    OrderId = order.Id,
                    BasketItemMessages = context.Message.BasketItemMessages,
                    BasketId = context.Message.BasketId,
                    CustomerId = context.Message.CustomerId,
                    PaymentMessage = context.Message.PaymentMessage,
                    ProductId = context.Message.BasketItemMessages.Select(bi => bi.ProductId).ToList()
                };
                
                await _publishEndpoint.Publish(orderCreatedEvent);
            }
        }
    }
}
