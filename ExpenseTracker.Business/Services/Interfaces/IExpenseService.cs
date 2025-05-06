using ExpenseTracker.Business.Dtos.Expense;

namespace ExpenseTracker.Business.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<int> CreateExpenseAsync(int userId, CreateExpenseRequestDto request, string? receiptPath);
        Task<List<ExpenseResponseDto>> GetMyExpensesAsync(int userId);
        Task<List<ExpenseResponseDto>> GetAllExpensesAsync();
        Task ApproveExpenseAsync(int approverId, ApproveExpenseDto dto);
        Task RejectExpenseAsync(int approverId, RejectExpenseDto dto);
        Task<List<ExpenseResponseDto>> FilterMyExpensesAsync(int userId, FilterExpenseRequestDto filter);
    }
}
