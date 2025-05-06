using System.Threading.Tasks;

public interface IReportService
{
    Task<List<PersonalExpenseReportDto>> GetPersonalReportAsync(int userId);

    Task<List<CompanyExpenseSummaryDto>> GetCompanyExpenseSummaryByMonthAsync(string period);

    Task<List<CompanyDailyExpenseDto>> GetCompanyDailySummaryByDateAsync(DateTime date);

    Task<List<CompanyWeeklyExpenseDto>> GetCompanyWeeklySummaryByWeekAsync(int year, int week);
    Task<List<PersonnelMonthlyExpenseDto>> GetPersonnelMonthlyExpenseSummaryFilteredAsync(int userId, DateTime start, DateTime end);
    Task<List<PersonnelDailyExpenseDto>> GetPersonnelDailyExpenseSummaryFilteredAsync(int userId, DateTime start, DateTime end);
    Task<List<PersonnelWeeklyExpenseDto>> GetPersonnelWeeklyExpenseSummaryFilteredAsync(int userId, DateTime start, DateTime end);
    
    Task<List<ExpenseStatusSummaryDto>> GetExpenseStatusSummaryByStatusAsync(DateTime start, DateTime end, int status);
}

