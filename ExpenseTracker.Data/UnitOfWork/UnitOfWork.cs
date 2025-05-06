using ExpenseTracker.Data.Context;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Data.Repositories.Implementations;
using ExpenseTracker.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace ExpenseTracker.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ExpenseTrackerDbContext _context;
        private IDbContextTransaction _transaction;

        public IRepository<User> Users { get; }
        public IRepository<Expense> Expenses { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<PaymentMethod> PaymentMethods { get; }

        public UnitOfWork(ExpenseTrackerDbContext context)
        {
            _context = context;

            Users = new Repository<User>(_context);
            Expenses = new Repository<Expense>(_context);
            Categories = new Repository<Category>(_context);
            PaymentMethods = new Repository<PaymentMethod>(_context);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
