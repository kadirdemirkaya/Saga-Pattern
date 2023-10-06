using System.Linq.Expressions;

namespace SharedLIBRARY.Repository.Generic
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveChangesAsync();
        Task<bool> Any(Expression<Func<T, bool>> expression);
        Task<T> GetFilter(Expression<Func<T, bool>> filter);
        Task<List<T>> GetAllFilter(Expression<Func<T, bool>> filter);
    }
}
