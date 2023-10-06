using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentAPI.Data;
using PaymentAPI.Dtos;
using PaymentAPI.Model;
using SharedLIBRARY.Repository.Generic;

namespace PaymentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentDbContext _dbContext;
        private readonly IRepository<Payment> _paymentRepository;
        private readonly IMapper _mapper;
        public PaymentController(PaymentDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _paymentRepository = new Repository<Payment>(dbContext);
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentDto paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);
            await _paymentRepository.AddAsync(payment);
            return Ok(payment);
        }
    }
}
