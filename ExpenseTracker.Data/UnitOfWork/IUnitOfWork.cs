using ExpenseTracker.Data.Entities;
using ExpenseTracker.Data.Repositories.Interfaces;

namespace ExpenseTracker.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Expense> Expenses { get; }
        IRepository<Category> Categories { get; }
        IRepository<PaymentMethod> PaymentMethods { get; }
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesAsync();

    }
}
