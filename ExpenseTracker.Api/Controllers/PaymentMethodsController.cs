using ExpenseTracker.Business.Dtos.PaymentMethod;
using ExpenseTracker.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodsController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _paymentMethodService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _paymentMethodService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentMethodDto request)
        {
            var id = await _paymentMethodService.CreateAsync(request);
            return Ok(new { message = "Ödeme yöntemi başarıyla oluşturuldu.", paymentMethodId = id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,UpdatePaymentMethodDto request)
        {
            await _paymentMethodService.UpdateAsync(id, request);
            return Ok(new { message = "Ödeme yöntemi güncellendi." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _paymentMethodService.DeleteAsync(id);
            return Ok(new { message = "Ödeme yöntemi silindi (soft delete)." });
        }
    }
}

