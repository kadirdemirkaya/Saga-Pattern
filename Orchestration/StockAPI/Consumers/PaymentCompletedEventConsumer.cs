using AutoMapper;
using MassTransit;
using SharedLIBRARY.Events;
using SharedLIBRARY.Repository.Generic;
using StockAPI.Data;
using StockAPI.Model;

namespace StockAPI.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly StockDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepository<Stock> _stockRepository;


        public PaymentCompletedEventConsumer(IPublishEndpoint publishEndpoint, StockDbContext dbContext)
        {
            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
            _stockRepository = new Repository<Stock>(dbContext);
        }
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            // !
        }
    }
}
