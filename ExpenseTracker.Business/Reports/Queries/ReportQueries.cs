public static class ReportQueries
{
    public const string GetPersonalExpenseReportFromView = @"
        SELECT 
            Id,
            Description,
            CategoryName,
            Amount,
            PaymentMethod,
            CreatedAt
        FROM vw_PersonalExpenseReport
        WHERE UserId = @UserId
        ORDER BY CreatedAt DESC";

    public const string GetCompanyExpenseSummaryByMonth =
     "EXEC sp_GetCompanyExpenseSummaryByMonth @Period";

    public const string GetCompanyDailyExpenseByDate =
    "EXEC sp_GetCompanyDailyExpenseSummaryByDate @Date";

    public const string GetCompanyWeeklyExpenseByWeek =
        "EXEC sp_GetCompanyWeeklyExpenseSummaryByWeek @Year, @Week";

    public const string GetPersonnelMonthlyExpenseSummaryFiltered =
        "EXEC sp_GetPersonnelMonthlyExpenseSummaryFiltered @UserId, @StartDate, @EndDate";
    public const string GetPersonnelDailyExpenseSummaryFiltered =
    "EXEC sp_GetPersonnelDailyExpenseSummaryFiltered @UserId, @StartDate, @EndDate";
    public const string GetPersonnelWeeklyExpenseSummaryFiltered =
    "EXEC sp_GetPersonnelWeeklyExpenseSummaryFiltered @UserId, @StartDate, @EndDate";
    public const string GetExpenseStatusSummaryByDateRangeFiltered =
    "EXEC sp_GetExpenseStatusSummaryByDateRangeFiltered @StartDate, @EndDate, @Status";


}
