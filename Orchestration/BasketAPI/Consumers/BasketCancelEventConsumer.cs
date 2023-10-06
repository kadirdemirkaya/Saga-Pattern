using AutoMapper;
using BasketAPI.Data;
using BasketAPI.Models;
using MassTransit;
using SharedLIBRARY.Events;
using SharedLIBRARY.Repository.Generic;

namespace BasketAPI.Consumers
{
    public class BasketCancelEventConsumer : IConsumer<BasketCancelEvent>
    {
        private readonly BasketDbContext _dbContext;
        private readonly IRepository<Basket> _basketRepository;
        private readonly IRepository<BasketItem> _basketItemRepository;

        public BasketCancelEventConsumer(BasketDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _basketRepository = new Repository<Basket>(dbContext);
            _basketItemRepository = new Repository<BasketItem>(dbContext);
        }

        public async Task Consume(ConsumeContext<BasketCancelEvent> context)
        {
            var basket = await _basketRepository.GetByIdAsync(context.Message.BasketId);
            basket.Status = SharedLIBRARY.Enums.BasketStatus.Fail;
            basket.ErrorMessage = "Basket Cancel Error !";

            var basketItems = await _basketItemRepository.GetAllFilter(bi => bi.BasketId == context.Message.BasketId);
            basketItems.ForEach(item => item.Status = SharedLIBRARY.Enums.BasketStatus.Fail);
            basketItems.ForEach(item => item.ErrorMessage = "Basket Cancel Error !");

            await _basketRepository.SaveChangesAsync();
        }
    }
}
