using Dapper;
using ExpenseTracker.Business.Logging.Interfaces;

public class ReportService : IReportService
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILoggingService<ReportService> _logger;

    public ReportService(IDbConnectionFactory connectionFactory, ILoggingService<ReportService> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<List<PersonalExpenseReportDto>> GetPersonalReportAsync(int userId)
    {
        _logger.LogInfo("Kişisel masraf raporu çağrıldı: UserId={UserId}", userId);

        using var connection = _connectionFactory.CreateConnection();
        var results = await connection.QueryAsync<PersonalExpenseReportDto>(
            ReportQueries.GetPersonalExpenseReportFromView,
            new { UserId = userId });

        return results.ToList();
    }

    public async Task<List<CompanyExpenseSummaryDto>> GetCompanyExpenseSummaryByMonthAsync(string period)
    {
        _logger.LogInfo("Şirket aylık masraf özeti sorgusu: Period={Period}", period);

        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryAsync<CompanyExpenseSummaryDto>(
            ReportQueries.GetCompanyExpenseSummaryByMonth,
            new { Period = period });

        return result.ToList();
    }

    public async Task<List<CompanyDailyExpenseDto>> GetCompanyDailySummaryByDateAsync(DateTime date)
    {
        _logger.LogInfo("Şirket günlük harcama özeti sorgusu: Date={Date}", date.ToShortDateString());

        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryAsync<CompanyDailyExpenseDto>(
            ReportQueries.GetCompanyDailyExpenseByDate,
            new { Date = date });

        return result.ToList();
    }

    public async Task<List<CompanyWeeklyExpenseDto>> GetCompanyWeeklySummaryByWeekAsync(int year, int week)
    {
        _logger.LogInfo("Şirket haftalık harcama özeti sorgusu: Year={Year}, Week={Week}", year, week);

        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryAsync<CompanyWeeklyExpenseDto>(
            ReportQueries.GetCompanyWeeklyExpenseByWeek,
            new { Year = year, Week = week });

        return result.ToList();
    }

    public async Task<List<PersonnelMonthlyExpenseDto>> GetPersonnelMonthlyExpenseSummaryFilteredAsync(int userId, DateTime start, DateTime end)
    {
        _logger.LogInfo("Personel aylık rapor sorgusu: UserId={UserId}, Start={Start}, End={End}",
            userId, start.ToShortDateString(), end.ToShortDateString());

        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryAsync<PersonnelMonthlyExpenseDto>(
            ReportQueries.GetPersonnelMonthlyExpenseSummaryFiltered,
            new { UserId = userId, StartDate = start, EndDate = end });

        return result.ToList();
    }

    public async Task<List<PersonnelDailyExpenseDto>> GetPersonnelDailyExpenseSummaryFilteredAsync(int userId, DateTime start, DateTime end)
    {
        _logger.LogInfo("Personel günlük rapor sorgusu: UserId={UserId}, Start={Start}, End={End}",
            userId, start.ToShortDateString(), end.ToShortDateString());

        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryAsync<PersonnelDailyExpenseDto>(
            ReportQueries.GetPersonnelDailyExpenseSummaryFiltered,
            new { UserId = userId, StartDate = start, EndDate = end });

        return result.ToList();
    }

    public async Task<List<PersonnelWeeklyExpenseDto>> GetPersonnelWeeklyExpenseSummaryFilteredAsync(int userId, DateTime start, DateTime end)
    {
        _logger.LogInfo("Personel haftalık rapor sorgusu: UserId={UserId}, Start={Start}, End={End}",
            userId, start.ToShortDateString(), end.ToShortDateString());

        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryAsync<PersonnelWeeklyExpenseDto>(
            ReportQueries.GetPersonnelWeeklyExpenseSummaryFiltered,
            new { UserId = userId, StartDate = start, EndDate = end });

        return result.ToList();
    }

    public async Task<List<ExpenseStatusSummaryDto>> GetExpenseStatusSummaryByStatusAsync(DateTime start, DateTime end, int status)
    {
        _logger.LogInfo("Duruma göre masraf özeti sorgusu: Status={Status}, Start={Start}, End={End}",
            status, start.ToShortDateString(), end.ToShortDateString());

        using var connection = _connectionFactory.CreateConnection();
        var result = await connection.QueryAsync<ExpenseStatusSummaryDto>(
            ReportQueries.GetExpenseStatusSummaryByDateRangeFiltered,
            new { StartDate = start, EndDate = end, Status = status });

        return result.ToList();
    }
}
