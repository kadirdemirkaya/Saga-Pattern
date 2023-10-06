using AutoMapper;
using MassTransit;
using SharedLIBRARY.Events;
using SharedLIBRARY.Repository.Generic;
using StockAPI.Data;
using StockAPI.Model;

namespace StockAPI.Consumers
{
    public class PaymentNotCompletedEventConsumer : IConsumer<PaymentNotCompletedEvent>
    {
        private readonly StockDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepository<Stock> _stockRepository;

        public PaymentNotCompletedEventConsumer(IPublishEndpoint publishEndpoint, StockDbContext dbContext)
        {
            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
            _stockRepository = new Repository<Stock>(dbContext);    
        }
        public async Task Consume(ConsumeContext<PaymentNotCompletedEvent> context)
        {
            foreach (var stockIds in context.Message.StockId)
            {
                var stock = await _stockRepository.GetByIdAsync(stockIds);
                stock.ErrorMessage = "Payment not completed event error";
                stock.Count += context.Message.Count;
            }
            await _stockRepository.SaveChangesAsync();
        }
    }
}
