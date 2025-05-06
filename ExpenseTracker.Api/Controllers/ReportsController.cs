using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [Authorize(Roles = "User")]
    [HttpGet("personal")]
    public async Task<IActionResult> GetPersonalReport()
    {
        var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = User.FindFirstValue(ClaimTypes.Role);
        Console.WriteLine($"UserId: {id}, Role: {role}");

        var result = await _reportService.GetPersonalReportAsync(int.Parse(id));
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("company/monthly-summary-by-month")]
    public async Task<IActionResult> GetCompanyExpenseSummaryByMonth([FromQuery] string period)
    {
        if (string.IsNullOrWhiteSpace(period))
            return BadRequest("Period query parameter is required. Example: 2025-04");

        var result = await _reportService.GetCompanyExpenseSummaryByMonthAsync(period);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("company/daily-summary-by-date")]
    public async Task<IActionResult> GetDailySummaryByDate([FromQuery] DateTime date)
    {
        if (date == default)
            return BadRequest("Please provide a valid date.");

        var result = await _reportService.GetCompanyDailySummaryByDateAsync(date);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("company/weekly-summary-by-week")]
    public async Task<IActionResult> GetWeeklySummaryByWeek([FromQuery] int year, [FromQuery] int week)
    {
        if (year <= 0 || week <= 0)
            return BadRequest("Please provide valid year and week parameters.");

        var result = await _reportService.GetCompanyWeeklySummaryByWeekAsync(year, week);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("company/personnel-summary-monthly")]
    public async Task<IActionResult> GetPersonnelSummary(
    [FromQuery] int userId,
    [FromQuery] DateTime start,
    [FromQuery] DateTime end)
    {
        if (userId <= 0 || start == default || end == default)
            return BadRequest("Lütfen geçerli userId ve tarih aralığı giriniz.");

        var result = await _reportService.GetPersonnelMonthlyExpenseSummaryFilteredAsync(userId, start, end);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("company/personnel-summary-daily")]
    public async Task<IActionResult> GetPersonnelDailySummary(
    [FromQuery] int userId,
    [FromQuery] DateTime start,
    [FromQuery] DateTime end)
    {
        if (userId <= 0 || start == default || end == default)
            return BadRequest("Lütfen geçerli userId ve tarih aralığı giriniz.");

        var result = await _reportService.GetPersonnelDailyExpenseSummaryFilteredAsync(userId, start, end);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("company/personnel-summary-weekly")]
    public async Task<IActionResult> GetPersonnelWeeklySummary(
    [FromQuery] int userId,
    [FromQuery] DateTime start,
    [FromQuery] DateTime end)
    {
        if (userId <= 0 || start == default || end == default)
            return BadRequest("Lütfen geçerli userId ve tarih aralığı giriniz.");

        var result = await _reportService.GetPersonnelWeeklyExpenseSummaryFilteredAsync(userId, start, end);
        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("company/status-summary")]
    public async Task<IActionResult> GetStatusSummary(
        [FromQuery] DateTime start,
        [FromQuery] DateTime end,
        [FromQuery] ReportStatusFilter status)
    {
        if (start == default || end == default)
            return BadRequest("Geçerli bir tarih aralığı giriniz.");

        var result = await _reportService.GetExpenseStatusSummaryByStatusAsync(start, end, (int)status);
        return Ok(result);
    }
}