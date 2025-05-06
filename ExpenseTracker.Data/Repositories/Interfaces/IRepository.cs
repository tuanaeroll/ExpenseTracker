using System.Linq.Expressions;
using ExpenseTracker.Data.Entities;

namespace ExpenseTracker.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task<List<Expense>> GetAllWithUserAsync();
        Task<List<Expense>> GetAllWithDetailsAsync();
        void Update(T entity);
        void Delete(T entity);
    }
}
