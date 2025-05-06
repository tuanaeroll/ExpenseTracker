using ExpenseTracker.Business.Dtos.User;
using ExpenseTracker.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyInfo()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _userService.GetUserByIdAsync(userId);
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequestDto request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _userService.UpdateUserAsync(userId, request);
            return Ok(new { message = "Bilgileriniz güncellendi." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.SoftDeleteUserAsync(id);
            return Ok(new { message = "Kullanıcı başarıyla silindi (soft delete)." });
        }

        [HttpPut("change-credentials")]
        public async Task<IActionResult> ChangeCredentials([FromBody] ChangeCredentialsRequestDto request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _userService.ChangeCredentialsAsync(userId, request);
            return Ok(new { message = "Bilgiler başarıyla güncellendi." });
        }
    }
}
