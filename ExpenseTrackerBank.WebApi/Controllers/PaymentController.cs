using Microsoft.AspNetCore.Mvc;
using ExpenseTrackerBank.WebApi.Dtos;
using ExpenseTrackerBank.WebApi.Services;
using ExpenseTrackerBank.WebApi.Interfaces;

namespace ExpenseTrackerBank.WebApi.Controllers
{
    [ApiController]
    [Route("api/expensetrackerbank/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> PostPayment([FromBody] PaymentRequestDto request)
        {
            var result = await _paymentService.SimulatePaymentAsync(request);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
