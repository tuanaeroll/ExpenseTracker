using ExpenseTracker.Business.Dtos.User;

namespace ExpenseTracker.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<int> RegisterUserAsync(RegisterUserRequestDto request);
        Task<int> RegisterAdminAsync(RegisterUserRequestDto request); 
        Task<string> LoginAsync(LoginRequestDto request);             
    }
}
