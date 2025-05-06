using AutoMapper;
using ExpenseManagement.Business.Interfaces;
using ExpenseManagement.Business.Models;
using ExpenseTracker.Business.Dtos.Expense;
using ExpenseTracker.Business.Logging.Interfaces;
using ExpenseTracker.Business.Services.Interfaces;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Data.Enums;
using ExpenseTracker.Data.UnitOfWork;

namespace ExpenseTracker.Business.Services.Implementations
{
    public class ExpenseService : IExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBankPaymentService _bankPaymentService;
        private readonly ILoggingService<ExpenseService> _logger;

        public ExpenseService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IBankPaymentService bankPaymentService,
            ILoggingService<ExpenseService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _bankPaymentService = bankPaymentService;
            _logger = logger;
        }

        public async Task<int> CreateExpenseAsync(int userId, CreateExpenseRequestDto request, string? receiptPath)
        {
            _logger.LogInfo("Masraf oluşturuluyor: UserId={UserId}, Title={Title}", userId, request.Title);

            var expense = _mapper.Map<Expense>(request);
            expense.UserId = userId;
            expense.Status = ExpenseStatus.Pending;
            expense.CreatedAt = DateTime.Now;
            expense.ReceiptFilePath = receiptPath;

            await _unitOfWork.Expenses.AddAsync(expense);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo("Masraf başarıyla oluşturuldu: ExpenseId={ExpenseId}", expense.Id);

            return expense.Id;
        }

        public async Task<List<ExpenseResponseDto>> GetMyExpensesAsync(int userId)
        {
            _logger.LogInfo("Kullanıcının masrafları getiriliyor: UserId={UserId}", userId);

            var allExpenses = await _unitOfWork.Expenses.GetAllWithDetailsAsync();

            var userExpenses = allExpenses
                .Where(e => e.UserId == userId)
                .ToList();

            return userExpenses.Select(e => new ExpenseResponseDto
            {
                Id = e.Id,
                Title = e.Title,
                Amount = e.Amount,
                Description = e.Description,
                CategoryName = e.Category?.Name ?? "",
                PaymentMethodName = e.PaymentMethod?.Name ?? "",
                UserFullName = $"{e.User.FirstName} {e.User.LastName}",
                Status = e.Status.ToString(),
                CreatedAt = e.CreatedAt
            }).ToList();
        }


        public async Task<List<ExpenseResponseDto>> GetAllExpensesAsync()
        {
            _logger.LogInfo("Tüm masraflar yönetici tarafından sorgulanıyor.");

            var expenses = await _unitOfWork.Expenses.GetAllWithDetailsAsync();

            return expenses.Select(e => new ExpenseResponseDto
            {
                Id = e.Id,
                Title = e.Title,
                Amount = e.Amount,
                Description = e.Description,
                CategoryName = e.Category?.Name ?? "",
                PaymentMethodName = e.PaymentMethod?.Name ?? "",
                UserFullName = $"{e.User.FirstName} {e.User.LastName}",
                Status = e.Status.ToString(),
                CreatedAt = e.CreatedAt
            }).ToList();
        }


        public async Task ApproveExpenseAsync(int approverId, ApproveExpenseDto dto)
        {
            _logger.LogInfo("Masraf onay işlemi başladı: ExpenseId={ExpenseId}, ApproverId={ApproverId}", dto.ExpenseId, approverId);

            var expense = await _unitOfWork.Expenses.GetByIdAsync(dto.ExpenseId);
            if (expense is null || !expense.IsActive)
            {
                _logger.LogWarning("Masraf bulunamadı veya pasif: ExpenseId={ExpenseId}", dto.ExpenseId);
                throw new Exception("Masraf bulunamadı.");
            }

            if (expense.Status != ExpenseStatus.Pending)
            {
                _logger.LogWarning("Masraf zaten onaylanmış/reddedilmiş: ExpenseId={ExpenseId}, Status={Status}", dto.ExpenseId, expense.Status);
                throw new Exception("Yalnızca beklemedeki masraflar onaylanabilir.");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(expense.UserId);
            if (user is null || string.IsNullOrWhiteSpace(user.IBAN))
            {
                _logger.LogWarning("Kullanıcı veya IBAN bilgisi eksik: UserId={UserId}", expense.UserId);
                throw new Exception("Kullanıcının IBAN bilgisi eksik.");
            }

            var paymentRequest = new BankPaymentRequestDto
            {
                Iban = user.IBAN,
                Amount = expense.Amount,
                FullName = $"{user.FirstName} {user.LastName}"
            };

            var paymentResult = await _bankPaymentService.MakePaymentAsync(paymentRequest);

            if (!paymentResult.Success)
            {
                _logger.LogError(new Exception("Simülasyon hatası"), "Ödeme başarısız: ExpenseId={ExpenseId}, Message={Message}", dto.ExpenseId, paymentResult.Message);
                throw new Exception($"Ödeme başarısız: {paymentResult.Message}");
            }

            UpdateExpenseAfterApproval(expense, approverId, paymentResult.TransactionId);

            _unitOfWork.Expenses.Update(expense);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo("Masraf onaylandı ve ödeme simüle edildi: ExpenseId={ExpenseId}, TransactionId={TransactionId}", expense.Id, expense.ReferenceCode);
        }

        private void UpdateExpenseAfterApproval(Expense expense, int approverId, string? transactionId)
        {
            expense.Status = ExpenseStatus.Approved;
            expense.ApprovedById = approverId;
            expense.PaidAt = DateTime.Now;
            expense.ReferenceCode = transactionId ?? $"REF-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }

        public async Task RejectExpenseAsync(int approverId, RejectExpenseDto dto)
        {
            _logger.LogInfo("Masraf reddediliyor: ExpenseId={ExpenseId}, ApproverId={ApproverId}", dto.ExpenseId, approverId);

            var expense = await _unitOfWork.Expenses.GetByIdAsync(dto.ExpenseId);

            if (expense == null || !expense.IsActive)
            {
                _logger.LogWarning("Masraf bulunamadı veya pasif: ExpenseId={ExpenseId}", dto.ExpenseId);
                throw new Exception("Masraf bulunamadı.");
            }

            if (expense.Status != ExpenseStatus.Pending)
            {
                _logger.LogWarning("Masraf zaten işleme alınmış: ExpenseId={ExpenseId}, Status={Status}", dto.ExpenseId, expense.Status);
                throw new Exception("Yalnızca beklemedeki masraflar reddedilebilir.");
            }

            expense.Status = ExpenseStatus.Rejected;
            expense.ApprovedById = approverId;
            expense.ResponseNote = dto.ResponseNote;

            _unitOfWork.Expenses.Update(expense);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo("Masraf reddedildi: ExpenseId={ExpenseId}", dto.ExpenseId);
        }

        public async Task<List<ExpenseResponseDto>> FilterMyExpensesAsync(int userId, FilterExpenseRequestDto filter)
        {
            _logger.LogInfo("Masraf filtreleme işlemi: UserId={UserId}", userId);

            var allExpenses = await _unitOfWork.Expenses.GetAllWithDetailsAsync();

            var query = allExpenses
                .Where(e => e.UserId == userId && e.IsActive)
                .AsQueryable();

            if (filter.CreatedAt.HasValue)
            {
                var date = filter.CreatedAt.Value.Date;
                var end = date.AddDays(1).AddTicks(-1);
                query = query.Where(e => e.CreatedAt >= date && e.CreatedAt <= end);
            }

            if (filter.PaidAt.HasValue)
            {
                var date = filter.PaidAt.Value.Date;
                var end = date.AddDays(1).AddTicks(-1);
                query = query.Where(e => e.PaidAt.HasValue && e.PaidAt >= date && e.PaidAt <= end);
            }

            if (filter.CategoryId.HasValue)
                query = query.Where(e => e.CategoryId == filter.CategoryId.Value);

            if (filter.PaymentMethodId.HasValue)
                query = query.Where(e => e.PaymentMethodId == filter.PaymentMethodId.Value);

            if (filter.Status.HasValue)
                query = query.Where(e => e.Status == filter.Status.Value);

            return query.Select(e => new ExpenseResponseDto
            {
                Id = e.Id,
                Title = e.Title,
                Amount = e.Amount,
                Description = e.Description,
                CategoryName = e.Category.Name ?? "",
                PaymentMethodName = e.PaymentMethod.Name ?? "",
                UserFullName = $"{e.User.FirstName} {e.User.LastName}",
                Status = e.Status.ToString(),
                CreatedAt = e.CreatedAt,
                PaidAt = e.PaidAt
            }).ToList();
        }
    }
}
