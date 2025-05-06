using System.Linq.Expressions;
using ExpenseTracker.Data.Context;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data.Repositories.Implementations
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ExpenseTrackerDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ExpenseTrackerDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<List<Expense>> GetAllWithUserAsync()
        {
            return await _context.Expenses
                .Include(e => e.User)
                .Where(e => e.IsActive)
                .ToListAsync();
        }

        public async Task<List<Expense>> GetAllWithDetailsAsync()
        {
            return await _context.Expenses
                .Include(e => e.User)
                .Include(e => e.Category)
                .Include(e => e.PaymentMethod)
                .Where(e => e.IsActive)
                .ToListAsync();
        }


        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            var property = entity.GetType().GetProperty("IsActive");
            if (property != null)
            {
                property.SetValue(entity, false);
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }
        }
    }
}
