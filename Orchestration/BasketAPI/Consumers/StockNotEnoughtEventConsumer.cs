using BasketAPI.Data;
using BasketAPI.Models;
using MassTransit;
using SharedLIBRARY.Events;
using SharedLIBRARY.Repository.Generic;

namespace BasketAPI.Consumers
{
    public class StockNotEnoughtEventConsumer : IConsumer<StockNotEnoughtEvent>
    {
        private readonly BasketDbContext _dbContext;
        private readonly IRepository<Basket> _basketRepository;
        private readonly IRepository<BasketItem> _basketItemRepository;

        public StockNotEnoughtEventConsumer(BasketDbContext dbContext)
        {
            _dbContext = dbContext;
            _basketRepository = new Repository<Basket>(dbContext);
            _basketItemRepository = new Repository<BasketItem>(dbContext);
        }

        public async Task Consume(ConsumeContext<StockNotEnoughtEvent> context)
        {
            Basket? basket = await _basketRepository.GetByIdAsync(context.Message.BasketId);
            basket.Status = SharedLIBRARY.Enums.BasketStatus.Fail;
            basket.ErrorMessage = "Stock Not Enought Error !";

            var basketItems = await _basketItemRepository.GetAllFilter(bi => bi.BasketId == context.Message.BasketId);
            basketItems.ForEach(item => item.Status = SharedLIBRARY.Enums.BasketStatus.Fail);
            basketItems.ForEach(item => item.ErrorMessage = "Stock Not Event Error !");

            await _basketRepository.SaveChangesAsync();
        }
    }
}
