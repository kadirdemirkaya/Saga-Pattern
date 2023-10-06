using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SharedLIBRARY.Repository.Generic;
using StockAPI.Data;
using StockAPI.Dtos;
using StockAPI.Model;

namespace StockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly StockDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IRepository<Stock> _stockRepository;

        public StockController(IMapper mapper, StockDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _stockRepository = new Repository<Stock>(dbContext);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody]StockDto stockDto)
        {
            var stock = _mapper.Map<Stock>(stockDto);
            await _stockRepository.AddAsync(stock);
            return Ok(stock);
        }
    }
}
