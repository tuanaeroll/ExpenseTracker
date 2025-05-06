using AutoMapper;
using ExpenseTracker.Business.Dtos.Category;
using ExpenseTracker.Business.Services.Interfaces;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Data.UnitOfWork;

namespace ExpenseTracker.Business.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.WhereAsync(c => c.IsActive);
            return categories.Select(c => _mapper.Map<CategoryResponseDto>(c)).ToList();
        }

        public async Task<CategoryResponseDto> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null || !category.IsActive)
                throw new Exception("Kategori bulunamadı.");

            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<int> CreateAsync(CreateCategoryDto request)
        {
            var category = _mapper.Map<Category>(request);
            category.CreatedAt = DateTime.Now;
            category.IsActive = true;

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return category.Id;
        }

        public async Task UpdateAsync(int id, UpdateCategoryDto request)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null || !category.IsActive)
                throw new Exception("Kategori bulunamadı.");

            category.Name = request.Name;
            category.Description = request.Description;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null || !category.IsActive)
                throw new Exception("Kategori bulunamadı.");

            var hasExpense = (await _unitOfWork.Expenses
                .WhereAsync(x => x.CategoryId == id && x.IsActive)).Any();

            if (hasExpense)
                throw new Exception("Bu kategoriye bağlı aktif masraf bulunduğu için silinemez.");

            category.IsActive = false;
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
