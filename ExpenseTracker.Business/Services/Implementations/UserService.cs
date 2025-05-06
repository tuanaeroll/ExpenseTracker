using AutoMapper;
using ExpenseTracker.Business.Dtos.User;
using ExpenseTracker.Business.Helpers;
using ExpenseTracker.Business.Logging.Interfaces;
using ExpenseTracker.Business.Services.Interfaces;
using ExpenseTracker.Data.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace ExpenseTracker.Business.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggingService<AuthService> _logger;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            ILoggingService<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            _logger.LogInfo("Kullanıcı bilgisi isteniyor: UserId={UserId}", id);

            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning("Kullanıcı bulunamadı: UserId={UserId}", id);
                throw new Exception("Kullanıcı bulunamadı.");
            }

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task UpdateUserAsync(int userId, UpdateUserRequestDto request)
        {
            _logger.LogInfo("Kullanıcı güncelleme işlemi başladı: UserId={UserId}", userId);

            var user = await _unitOfWork.Users.GetByIdAsync(userId);

            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("Güncelleme başarısız: Kullanıcı bulunamadı veya pasif. UserId={UserId}", userId);
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!string.IsNullOrWhiteSpace(request.FirstName))
                user.FirstName = request.FirstName;

            if (!string.IsNullOrWhiteSpace(request.LastName))
                user.LastName = request.LastName;

            if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                user.PhoneNumber = request.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(request.Position))
                user.Position = request.Position;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo("Kullanıcı güncellendi: UserId={UserId}", userId);
        }


        public async Task SoftDeleteUserAsync(int id)
        {
            _logger.LogInfo("Kullanıcı silme isteği: UserId={UserId}", id);

            var user = await _unitOfWork.Users.GetByIdAsync(id);

            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("Silme başarısız: Kullanıcı bulunamadı veya zaten pasif. UserId={UserId}", id);
                throw new Exception("Kullanıcı bulunamadı.");
            }

            user.IsActive = false;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo("Kullanıcı pasifleştirildi (soft delete): UserId={UserId}", id);
        }
        public async Task ChangeCredentialsAsync(int userId, ChangeCredentialsRequestDto request)
        {
            _logger.LogInfo("Kullanıcı bilgilerinde değişiklik isteği: UserId={UserId}", userId);

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null || !user.IsActive)
            {
                _logger.LogWarning("Kullanıcı bulunamadı veya pasif: UserId={UserId}", userId);
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!PasswordHelper.VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                _logger.LogWarning("Şifre doğrulama başarısız: UserId={UserId}", userId);
                throw new Exception("Mevcut şifre hatalı.");
            }

            if (!string.IsNullOrWhiteSpace(request.NewEmail))
            {
                var existing = await _unitOfWork.Users.WhereAsync(x => x.Email == request.NewEmail && x.Id != userId);
                if (existing.Any())
                    throw new Exception("Bu e-posta adresi zaten kullanılıyor.");

                user.Email = request.NewEmail;
            }

            if (!string.IsNullOrWhiteSpace(request.NewPassword))
            {
                user.PasswordHash = PasswordHelper.HashPassword(request.NewPassword);
            }

            _unitOfWork.Users.Update(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo("Kullanıcı bilgileri güncellendi: UserId={UserId}", userId);
        }

    }
}
