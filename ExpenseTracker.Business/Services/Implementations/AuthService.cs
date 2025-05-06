using AutoMapper;
using ExpenseTracker.Business.Dtos.User;
using ExpenseTracker.Business.Helpers;
using ExpenseTracker.Business.Logging.Interfaces;
using ExpenseTracker.Business.Services.Interfaces;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Data.Enums;
using ExpenseTracker.Data.UnitOfWork;
using Microsoft.Extensions.Configuration;

namespace ExpenseTracker.Business.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILoggingService<AuthService> _logger;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration,
            ILoggingService<AuthService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<int> RegisterUserAsync(RegisterUserRequestDto request)
        {
            _logger.LogInfo("Yeni kullanıcı kayıt denemesi: Email={Email}", request.Email);

            var user = _mapper.Map<User>(request);
            user.PasswordHash = PasswordHelper.HashPassword(request.Password);
            user.Role = Role.User;

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo("Kayıt başarılı: KullanıcıId={UserId}, Email={Email}", user.Id, user.Email);
            return user.Id;
        }

        public async Task<int> RegisterAdminAsync(RegisterUserRequestDto request)
        {
            _logger.LogInfo("Yeni admin kayıt denemesi: Email={Email}", request.Email);

            var user = _mapper.Map<User>(request);
            user.PasswordHash = PasswordHelper.HashPassword(request.Password);
            user.Role = Role.Admin;

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInfo("Admin kaydı başarılı: KullanıcıId={UserId}, Email={Email}", user.Id, user.Email);
            return user.Id;
        }

        public async Task<string> LoginAsync(LoginRequestDto request)
        {
            _logger.LogInfo("Giriş denemesi başlatıldı: Email={Email}", request.Email);

            var user = (await _unitOfWork.Users
                .WhereAsync(x => x.Email == request.Email && x.IsActive))
                .FirstOrDefault();

            if (user == null)
            {
                _logger.LogWarning("Giriş başarısız: Kullanıcı bulunamadı. Email={Email}", request.Email);
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (!PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Giriş başarısız: Şifre hatalı. Email={Email}", request.Email);
                throw new Exception("Şifre hatalı.");
            }

            var jwtSettings = _configuration.GetSection("Jwt").Get<JwtSettings>();

            var token = JwtHelper.GenerateJwtToken(
                user,
                jwtSettings.SecretKey,
                jwtSettings.Issuer,
                jwtSettings.Audience,
                jwtSettings.ExpireDays
            );

            _logger.LogInfo("Giriş başarılı: KullanıcıId={UserId}, Email={Email}", user.Id, user.Email);
            return token;
        }
    }
}
