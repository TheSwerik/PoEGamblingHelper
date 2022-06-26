using Backend.Model;

namespace Backend.Data;

public interface IRepository<T> where T : Entity
{
    IAsyncEnumerable<T> GetAll();
    Task<T?> Get(Guid id);
    Task<T> Save(T entity);
    Task<T> Update(T entity);
    void Delete(Guid id);
    void SaveChanges();
}