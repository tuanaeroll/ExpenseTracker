using ExpenseTracker.Business.Dtos.Expense;
using ExpenseTracker.Business.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpensesController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        [HttpGet("my-expenses")]
        public async Task<IActionResult> GetMyExpenses()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var expenses = await _expenseService.GetMyExpensesAsync(userId);
            return Ok(expenses);
        }

        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateExpenseRequestDto request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            string? receiptPath = null;

            if (request.Receipt != null && request.Receipt.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueName = $"{Guid.NewGuid()}_{Path.GetFileName(request.Receipt.FileName)}";
                var filePath = Path.Combine(uploadsFolder, uniqueName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await request.Receipt.CopyToAsync(stream);

                receiptPath = $"/uploads/{uniqueName}";
            }

            var expenseId = await _expenseService.CreateExpenseAsync(userId, request, receiptPath);

            return Ok(new { message = "Masraf oluşturuldu.", expenseId });
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllExpenses()
        {
            var result = await _expenseService.GetAllExpensesAsync();
            return Ok(result);
        }

        [HttpPost("approve")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(ApproveExpenseDto dto)
        {
            var approverId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _expenseService.ApproveExpenseAsync(approverId, dto);
            return Ok(new { message = "Masraf başarıyla onaylandı ve ödeme simülasyonu yapıldı." });
        }

        [HttpPost("reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(RejectExpenseDto dto)
        {
            var approverId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _expenseService.RejectExpenseAsync(approverId, dto);
            return Ok(new { message = "Masraf başarıyla reddedildi." });
        }

        [HttpPost("filter-my-expenses")]
        public async Task<IActionResult> FilterMyExpenses([FromBody] FilterExpenseRequestDto filter)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _expenseService.FilterMyExpensesAsync(userId, filter);
            return Ok(result);
        }
    }
}
