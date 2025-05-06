using ExpenseTrackerBank.WebApi.Dtos;
using ExpenseTrackerBank.WebApi.Interfaces;

namespace ExpenseTrackerBank.WebApi.Services
{
    public class PaymentService : IPaymentService
    {
        public async Task<PaymentResponseDto> SimulatePaymentAsync(PaymentRequestDto request)
        {
            await Task.Delay(1000);

            return new PaymentResponseDto
            {
                Success = true,
                TransactionId = $"TXN-{Guid.NewGuid().ToString().Substring(0, 8)}",
                Message = "Ödeme başarılı şekilde simüle edildi."
            };
        }
    }
}
