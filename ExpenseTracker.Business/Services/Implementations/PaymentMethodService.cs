using AutoMapper;
using ExpenseTracker.Business.Dtos.PaymentMethod;
using ExpenseTracker.Business.Services.Interfaces;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Data.UnitOfWork;

namespace ExpenseTracker.Business.Services.Implementations
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentMethodService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<PaymentMethodResponseDto>> GetAllAsync()
        {
            var methods = await _unitOfWork.PaymentMethods.WhereAsync(p => p.IsActive);
            return methods.Select(p => _mapper.Map<PaymentMethodResponseDto>(p)).ToList();
        }

        public async Task<PaymentMethodResponseDto> GetByIdAsync(int id)
        {
            var method = await _unitOfWork.PaymentMethods.GetByIdAsync(id);

            if (method == null || !method.IsActive)
                throw new Exception("Ödeme yöntemi bulunamadı.");

            return _mapper.Map<PaymentMethodResponseDto>(method);
        }

        public async Task<int> CreateAsync(CreatePaymentMethodDto request)
        {
            var method = _mapper.Map<PaymentMethod>(request);
            method.CreatedAt = DateTime.Now;
            method.IsActive = true;

            await _unitOfWork.PaymentMethods.AddAsync(method);
            await _unitOfWork.SaveChangesAsync();

            return method.Id;
        }

        public async Task UpdateAsync(int id, UpdatePaymentMethodDto request)
        {
            var method = await _unitOfWork.PaymentMethods.GetByIdAsync(id);

            if (method == null || !method.IsActive)
                throw new Exception("Ödeme yöntemi bulunamadı.");

            method.Name = request.Name;
            method.Description = request.Description;

            _unitOfWork.PaymentMethods.Update(method);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var method = await _unitOfWork.PaymentMethods.GetByIdAsync(id);

            if (method == null || !method.IsActive)
                throw new Exception("Ödeme yöntemi bulunamadı.");

            var hasExpense = (await _unitOfWork.Expenses
    .WhereAsync(x => x.PaymentMethodId == id && x.IsActive)).Any();

            if (hasExpense)
                throw new Exception("Bu ödeme yöntemiyle ilişkili aktif masraf bulunduğu için silinemez.");

            method.IsActive = false;

            _unitOfWork.PaymentMethods.Update(method);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
