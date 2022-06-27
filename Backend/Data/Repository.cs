using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class Repository<T> : IRepository<T> where T : Entity
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly DbSet<T> _entities;

    public Repository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _entities = _applicationDbContext.Set<T>();
    }

    public IAsyncEnumerable<T> GetAll() { return _entities.AsAsyncEnumerable(); }

    public async Task<T?> Get(Guid id) { return await _entities.FindAsync(id); }

    public async Task<T> Save(T entity)
    {
        var result = _entities.Add(entity);
        await _applicationDbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task Save(IEnumerable<T> entities)
    {
        _entities.AddRange(entities);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<T> Update(T entity)
    {
        var result = _entities.Update(entity);
        await _applicationDbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async void Delete(Guid id)
    {
        var entity = await _entities.FindAsync(id);
        if (entity == null) return; // idempotency
        _entities.Remove(entity);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async void SaveChanges() { await _applicationDbContext.SaveChangesAsync(); }
}