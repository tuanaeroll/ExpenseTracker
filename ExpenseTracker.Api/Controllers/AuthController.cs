using ExpenseTracker.Business.Dtos.User;
using ExpenseTracker.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _userService;

        public AuthController(IAuthService userService)
        {
            _userService = userService;
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequestDto request)
        {
            var id = await _userService.RegisterUserAsync(request);
            return Ok(new { message = "Kullanıcı başarıyla oluşturuldu.", userId = id });
        }

        [HttpPost("register-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin(RegisterUserRequestDto request)
        {
            var id = await _userService.RegisterAdminAsync(request);
            return Ok(new { message = "Admin başarıyla oluşturuldu.", userId = id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var token = await _userService.LoginAsync(request);
            return Ok(new { token });
        }
    }
}
