-- 1. PERSONEL BAZLI RAPOR (VIEW)
CREATE VIEW vw_PersonalExpenseReport AS
SELECT 
    e.Id,
    e.Description,
    c.Name AS CategoryName,
    e.Amount,
    p.Name AS PaymentMethod,
    e.CreatedAt,
    e.UserId
FROM Expenses e
INNER JOIN Categories c ON e.CategoryId = c.Id
INNER JOIN PaymentMethods p ON e.PaymentMethodId = p.Id;
GO

-- 2. ŞİRKET AY BAZLI HARCAMA TUTARI (SP)
CREATE PROCEDURE sp_GetCompanyExpenseSummaryByMonth
    @Period NVARCHAR(7) -- Örn: '2025-04'
AS
BEGIN
    SELECT 
        FORMAT(e.CreatedAt, 'yyyy-MM') AS Period,
        SUM(e.Amount) AS TotalAmount
    FROM Expenses e
    WHERE FORMAT(e.CreatedAt, 'yyyy-MM') = @Period
      AND e.Status = 1
    GROUP BY FORMAT(e.CreatedAt, 'yyyy-MM');
END
GO

-- 3. ŞİRKET GÜN BAZLI HARCAMA TUTARI (SP)
CREATE OR ALTER PROCEDURE sp_GetCompanyDailyExpenseSummaryByDate
    @Date DATE
AS
BEGIN
    SELECT 
        CAST(e.CreatedAt AS DATE) AS [Date],
        SUM(e.Amount) AS TotalAmount
    FROM Expenses e
    WHERE e.Status = 1 AND CAST(e.CreatedAt AS DATE) = @Date
    GROUP BY CAST(e.CreatedAt AS DATE);
END
GO

-- 4. ŞİRKET HAFTA BAZLI HARCAMA TUTARI (SP)
CREATE OR ALTER PROCEDURE sp_GetCompanyWeeklyExpenseSummaryByWeek
    @Year INT,
    @Week INT
AS
BEGIN
    SELECT 
        CONCAT(YEAR(e.CreatedAt), '-', DATEPART(WEEK, e.CreatedAt)) AS Week,
        SUM(e.Amount) AS TotalAmount
    FROM Expenses e
    WHERE e.Status = 1
      AND YEAR(e.CreatedAt) = @Year
      AND DATEPART(WEEK, e.CreatedAt) = @Week
    GROUP BY YEAR(e.CreatedAt), DATEPART(WEEK, e.CreatedAt);
END
GO

-- 5. PERSONEL AYLIK HARCAMA (SP)
CREATE OR ALTER PROCEDURE sp_GetPersonnelMonthlyExpenseSummaryFiltered
    @UserId INT,
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        u.Id AS UserId,
        CONCAT(u.FirstName, ' ', u.LastName) AS FullName,
        FORMAT(e.CreatedAt, 'yyyy-MM') AS Period,
        SUM(e.Amount) AS TotalAmount
    FROM Expenses e
    INNER JOIN Users u ON e.UserId = u.Id
    WHERE 
        e.Status = 1
        AND e.UserId = @UserId
        AND e.CreatedAt BETWEEN @StartDate AND @EndDate
    GROUP BY u.Id, u.FirstName, u.LastName, FORMAT(e.CreatedAt, 'yyyy-MM')
    ORDER BY Period DESC;
END
GO

-- 6. PERSONEL GÜNLÜK HARCAMA (SP)
CREATE OR ALTER PROCEDURE sp_GetPersonnelDailyExpenseSummaryFiltered
    @UserId INT,
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        u.Id AS UserId,
        CONCAT(u.FirstName, ' ', u.LastName) AS FullName,
        CAST(e.CreatedAt AS DATE) AS [Date],
        SUM(e.Amount) AS TotalAmount
    FROM Expenses e
    INNER JOIN Users u ON e.UserId = u.Id
    WHERE 
        e.Status = 1
        AND e.UserId = @UserId
        AND e.CreatedAt BETWEEN @StartDate AND @EndDate
    GROUP BY u.Id, u.FirstName, u.LastName, CAST(e.CreatedAt AS DATE)
    ORDER BY [Date] DESC;
END
GO

-- 7. PERSONEL HAFTALIK HARCAMA (SP)
CREATE OR ALTER PROCEDURE sp_GetPersonnelWeeklyExpenseSummaryFiltered
    @UserId INT,
    @StartDate DATE,
    @EndDate DATE
AS
BEGIN
    SELECT 
        u.Id AS UserId,
        CONCAT(u.FirstName, ' ', u.LastName) AS FullName,
        CONCAT(YEAR(e.CreatedAt), '-', DATEPART(WEEK, e.CreatedAt)) AS Week,
        SUM(e.Amount) AS TotalAmount
    FROM Expenses e
    INNER JOIN Users u ON e.UserId = u.Id
    WHERE 
        e.Status = 1
        AND e.UserId = @UserId
        AND e.CreatedAt BETWEEN @StartDate AND @EndDate
    GROUP BY u.Id, u.FirstName, u.LastName, YEAR(e.CreatedAt), DATEPART(WEEK, e.CreatedAt)
    ORDER BY Week DESC;
END
GO

-- 8. MASRAF DURUMUNA GÖRE HARCAMA RAPORU (APPROVED / REJECTED)
CREATE OR ALTER PROCEDURE sp_GetExpenseStatusSummaryByDateRangeFiltered
    @StartDate DATE,
    @EndDate DATE,
    @Status INT -- 1: Approved, 2: Rejected
AS
BEGIN
    SELECT 
        CASE e.Status 
            WHEN 1 THEN 'Approved'
            WHEN 2 THEN 'Rejected'
            ELSE 'Other'
        END AS Status,
        FORMAT(e.CreatedAt, 'yyyy-MM') AS Period,
        SUM(e.Amount) AS TotalAmount
    FROM Expenses e
    WHERE 
        e.CreatedAt BETWEEN @StartDate AND @EndDate
        AND e.Status = @Status
    GROUP BY 
        FORMAT(e.CreatedAt, 'yyyy-MM'),
        e.Status
    ORDER BY Period DESC;
END