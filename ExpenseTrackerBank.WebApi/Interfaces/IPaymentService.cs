using ExpenseTrackerBank.WebApi.Dtos;

namespace ExpenseTrackerBank.WebApi.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> SimulatePaymentAsync(PaymentRequestDto request);
    }
}

