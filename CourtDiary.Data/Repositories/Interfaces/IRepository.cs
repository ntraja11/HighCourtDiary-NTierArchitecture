using System.Linq.Expressions;

namespace CourtDiary.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T?> GetAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);

        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
