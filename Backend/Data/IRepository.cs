using Microsoft.EntityFrameworkCore;
using Model;

namespace Backend.Data;

public interface IRepository<T> where T : CustomIdEntity
{
    IAsyncEnumerable<T> GetAllAsync();
    IEnumerable<T> GetAll();
    Task<T?> Get(Guid id);
    T? Get(Func<DbSet<T>, T?> function);
    Task<T> Save(T entity);
    Task Save(IEnumerable<T> entity);
    Task<T> Update(T entity);
    Task Update(IEnumerable<T> entity);
    void Delete(Guid id);
    void SaveChanges();
    IAsyncEnumerable<T> GetAllAsync(Func<DbSet<T>, IAsyncEnumerable<T>> function);
    IEnumerable<T> GetAll(Func<DbSet<T>, IEnumerable<T>> function);
    void ClearTrackedEntities();
}