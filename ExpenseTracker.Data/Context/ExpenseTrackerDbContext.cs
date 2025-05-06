using ExpenseTracker.Data.Entities;
using ExpenseTracker.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data.Context
{
    public class ExpenseTrackerDbContext : DbContext
    {
        public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<User>().HasQueryFilter(x => x.IsActive);

            modelBuilder.Entity<User>().HasData(
    new User
    {
        Id = 1,
        FirstName = "Admin",
        LastName = "User",
        Email = "admin@example.com",
        PasswordHash = "$2a$12$I7VPOGCd8BWmgIQHW.voE.gHwwB/vjaohE0uYKCznI8RcSD6Gcm7y",
        Role = Role.Admin,
        Position = "Yönetici",
        IBAN = "TR100000000000000000000001",
        PhoneNumber = "5000000000",
        IsActive = true,
        CreatedAt = new DateTime(2025, 05, 01, 00, 00, 00)
    },
    new User
    {
        Id = 2,
        FirstName = "Test",
        LastName = "User",
        Email = "test@example.com",
        PasswordHash = "$2a$12$7BC5tHAgjbAyVeMdm8i81.jr01PxwwFS8Y9tl1nl55gXMpEEPIh46",
        Role = Role.User,
        Position = "Saha Personeli",
        IBAN = "TR100000000000000000000002",
        PhoneNumber = "5000000001",
        IsActive = true,
        CreatedAt = new DateTime(2025, 05, 01, 00, 00, 00)
    }
);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.ApprovedBy)
                .WithMany()
                .HasForeignKey(e => e.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}
