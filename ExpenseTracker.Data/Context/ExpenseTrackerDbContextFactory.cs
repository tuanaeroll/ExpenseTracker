using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ExpenseTracker.Data.Context;

public class ExpenseTrackerDbContextFactory : IDesignTimeDbContextFactory<ExpenseTrackerDbContext>
{
    public ExpenseTrackerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ExpenseTrackerDbContext>();
        optionsBuilder.UseSqlServer("Server=TUANAEROL;Database=ExpenseTrackerDb;Trusted_Connection=True;TrustServerCertificate=True;");

        return new ExpenseTrackerDbContext(optionsBuilder.Options);
    }
}

