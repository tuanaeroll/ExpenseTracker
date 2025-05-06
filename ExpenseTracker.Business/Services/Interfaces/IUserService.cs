using ExpenseTracker.Business.Dtos.User;

namespace ExpenseTracker.Business.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task UpdateUserAsync(int userId, UpdateUserRequestDto request);
        Task SoftDeleteUserAsync(int id);
        Task ChangeCredentialsAsync(int userId, ChangeCredentialsRequestDto request);

    }
}
