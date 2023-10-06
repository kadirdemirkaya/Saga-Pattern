using AutoMapper;
using MassTransit;
using SharedLIBRARY.Events;
using SharedLIBRARY.Message;
using SharedLIBRARY.QueueEventNames;
using SharedLIBRARY.Repository.Generic;
using StockAPI.Data;
using StockAPI.Model;

namespace StockAPI.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly StockDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepository<Stock> _stockRepository;

        public OrderCreatedEventConsumer(IPublishEndpoint publishEndpoint, StockDbContext dbContext)
        {
            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
            _stockRepository = new Repository<Stock>(dbContext);
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> existStock = new();
            List<bool> validStock = new();
            List<int> stockIds = new();
            foreach (var basketItemMessage in context.Message.BasketItemMessages)
                existStock.Add(await _stockRepository.Any(s => s.ProductId == basketItemMessage.ProductId));
            foreach (var basketItemMessage in context.Message.BasketItemMessages)
            {
                var oldstock = await _stockRepository.GetFilter(s => s.ProductId == basketItemMessage.ProductId);
                if (basketItemMessage.Count == null || oldstock == null)
                    validStock.Add(false);
                else if (basketItemMessage.Count != null && oldstock.Count != null && oldstock.Count > basketItemMessage.Count)
                    validStock.Add(true);
                else
                    validStock.Add(false);
            }

            if (existStock.All(s => s.Equals(true)) && validStock.All(s => s.Equals(true)))
            {
                Stock? stock = new();
                foreach (var basketItemMessage in context.Message.BasketItemMessages)
                {
                    stock = await _stockRepository.GetFilter(s => s.ProductId == basketItemMessage.ProductId);
                    if (stock is not null)
                    {
                        stockIds.Add(stock.Id);
                        stock.Count -= basketItemMessage.Count;
                        await _stockRepository.SaveChangesAsync();
                    }
                }
                //send event 
                StockEnoughtEvent stockEnoughtEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    BasketId = context.Message.BasketId,
                    CustomerId = context.Message.CustomerId,
                    PaymentMessage = context.Message.PaymentMessage,
                    BasketItemMessages = context.Message.BasketItemMessages,
                    StockId = stockIds
                };

                #region 
                //await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{EventQueues.StockEnoughtEventConsumer}"));
                //await _sendEndpointProvider.Send(stockEnoughtEvent);
                #endregion
                await _publishEndpoint.Publish(stockEnoughtEvent);
            }
            else
            {
                StockNotEnoughtEvent stockNotEnoughtEvent = new()
                {
                    BasketId = context.Message.BasketId,
                    OrderId = context.Message.OrderId
                };
                await _publishEndpoint.Publish(stockNotEnoughtEvent);
            }
        }
    }
}
