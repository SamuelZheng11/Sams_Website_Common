
using System.Linq.Expressions;

namespace SamsWebsite.Common
{
    public interface IRepository<T> where T : IEntity
    {
        Task<T> CreateAsync(T entityToCreate);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetAsync(Guid id);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<T> UpdateAsync(T entityToUpdate);
        Task RemoveAsync(Guid id);
    }
}