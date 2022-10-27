using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public class Repository<T> : IRepository<T> where T : CustomIdEntity
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly DbSet<T> _entities;

    public Repository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _entities = _applicationDbContext.Set<T>();
    }

    public IAsyncEnumerable<T> GetAllAsync() { return _entities.AsAsyncEnumerable(); }
    public IEnumerable<T> GetAll() { return _entities; }

    public IAsyncEnumerable<T> GetAll(Func<DbSet<T>, IAsyncEnumerable<T>> function)
    {
        return function.Invoke(_entities);
    }

    public IEnumerable<T> GetAll(Func<DbSet<T>, IEnumerable<T>> function) { return function.Invoke(_entities); }
    public void ClearTrackedEntities() { _applicationDbContext.ChangeTracker.Clear(); }

    public async Task<T?> Get(Guid id) { return await _entities.FindAsync(id); }
    public T? Get(Func<DbSet<T>, T?> function) { return function.Invoke(_entities); }

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

    public async Task Update(IEnumerable<T> entities)
    {
        _entities.UpdateRange(entities);
        await _applicationDbContext.SaveChangesAsync();
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