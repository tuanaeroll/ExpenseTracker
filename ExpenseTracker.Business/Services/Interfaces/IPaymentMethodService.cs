using ExpenseTracker.Business.Dtos.PaymentMethod;

namespace ExpenseTracker.Business.Services.Interfaces
{
    public interface IPaymentMethodService
    {
        Task<List<PaymentMethodResponseDto>> GetAllAsync();
        Task<PaymentMethodResponseDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreatePaymentMethodDto request);
        Task UpdateAsync(int id, UpdatePaymentMethodDto request);
        Task DeleteAsync(int id);
    }
}
