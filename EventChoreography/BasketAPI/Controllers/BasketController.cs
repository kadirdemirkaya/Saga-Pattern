using AutoMapper;
using BasketAPI.Data;
using BasketAPI.Dtos;
using BasketAPI.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedLIBRARY.Enums;
using SharedLIBRARY.Events;
using SharedLIBRARY.Message;
using SharedLIBRARY.Repository.Generic;

namespace BasketAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRepository<Basket> _basketRepository;
        private readonly BasketDbContext _basketDbContext;
        private readonly IMapper _mapper;
        public BasketController(BasketDbContext basketDbContext, IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _basketDbContext = basketDbContext;
            _basketRepository = new Repository<Basket>(basketDbContext);
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmBasket([FromBody] ConfirmBasketDto confirmBasketDto)
        {
            var basket = Basket.Create(confirmBasketDto.CustomerId,SharedLIBRARY.Enums.BasketStatus.Uncertain,string.Empty);

            confirmBasketDto.BasketItemDtos.ForEach(item => basket.AddBasketItem(item.ProductId,item.BasketId, item.Price, item.Count,BasketStatus.Uncertain));

            await _basketRepository.AddAsync(basket);
            await _basketRepository.SaveChangesAsync();

            var basketConfirmedEvent = new BasketConfirmedEvent
            {
                BasketId = basket.Id,
                CustomerId = basket.CustomerId,
                AddressMessage = _mapper.Map<AddressMessage>(confirmBasketDto.AddressDto),
                PaymentMessage = _mapper.Map<PaymentMessage>(confirmBasketDto.PaymentDto),
                BasketItemMessages = _mapper.Map<List<BasketItemMessage>>(confirmBasketDto.BasketItemDtos)
            };

            await _publishEndpoint.Publish(basketConfirmedEvent);

            return Ok();
        }
    }
}
