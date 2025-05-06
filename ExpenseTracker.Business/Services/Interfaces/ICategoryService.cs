using ExpenseTracker.Business.Dtos.Category;

namespace ExpenseTracker.Business.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseDto>> GetAllAsync();
        Task<CategoryResponseDto> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateCategoryDto request);
        Task UpdateAsync(int id, UpdateCategoryDto request);
        Task DeleteAsync(int id);
    }
}
