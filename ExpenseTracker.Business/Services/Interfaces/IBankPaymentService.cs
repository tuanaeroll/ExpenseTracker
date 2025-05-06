using ExpenseManagement.Business.Models;

namespace ExpenseManagement.Business.Interfaces
{
    public interface IBankPaymentService
    {
        Task<BankPaymentResponseDto> MakePaymentAsync(BankPaymentRequestDto request);
    }
}
