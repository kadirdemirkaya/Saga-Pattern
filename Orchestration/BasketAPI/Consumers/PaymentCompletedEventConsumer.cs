using BasketAPI.Data;
using BasketAPI.Models;
using MassTransit;
using SharedLIBRARY.Events;
using SharedLIBRARY.Repository.Generic;

namespace BasketAPI.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly BasketDbContext _dbContext;
        private readonly IRepository<Basket> _basketRepository;
        private readonly IRepository<BasketItem> _basketItemRepository;

        public PaymentCompletedEventConsumer( BasketDbContext dbContext)
        {
            _dbContext = dbContext;
            _basketRepository = new Repository<Basket>(dbContext);
            _basketItemRepository = new Repository<BasketItem>(dbContext);
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var basket = await _basketRepository.GetByIdAsync(context.Message.BasketId);
            var basketItem = await _basketItemRepository.GetAllFilter(bi => bi.BasketId == context.Message.BasketId);

            basket.Status = SharedLIBRARY.Enums.BasketStatus.Complete;
            basketItem.ForEach(bi => bi.Status = SharedLIBRARY.Enums.BasketStatus.Complete);

            await _basketRepository.SaveChangesAsync();

        }
    }
}
