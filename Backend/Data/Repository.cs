using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.QueryParameters;

namespace Backend.Data;

public class Repository<T, TKey> : IRepository<T, TKey> where T : class, IEntity
{
    private readonly ApplicationDbContext _applicationDbContext;
    protected readonly DbSet<T> Entities;

    public Repository(ApplicationDbContext applicationDbContext)
    {
        if (typeof(TKey) != T.KeyType()) throw new UnreachableException("TKey does not match KeyType of T.");
        _applicationDbContext = applicationDbContext;
        Entities = _applicationDbContext.Set<T>();
    }

    public IAsyncEnumerable<T> GetAllAsync() { return Entities.AsAsyncEnumerable(); }
    public IEnumerable<T> GetAll() { return Entities; }

    public IAsyncEnumerable<T> GetAllAsync(Func<DbSet<T>, IQueryable<T>> function)
    {
        return function.Invoke(Entities).AsAsyncEnumerable();
    }

    public IAsyncEnumerable<T> GetAllAsync(Page? page, Func<DbSet<T>, IQueryable<T>> function)
    {
        if (page is null) return GetAllAsync(function);
        var pageSize = page.PageSize ?? 0;
        var takeSize = page.PageSize ?? int.MaxValue;
        return function.Invoke(Entities).Skip(pageSize * page.PageNumber).Take(takeSize).AsAsyncEnumerable();
    }

    public IEnumerable<T> GetAll(Func<DbSet<T>, IEnumerable<T>> function) { return function.Invoke(Entities); }

    public IEnumerable<T> GetAll(Page? page, Func<DbSet<T>, IEnumerable<T>> function)
    {
        if (page is null) return GetAll(function);
        var pageSize = page.PageSize ?? 0;
        var takeSize = page.PageSize ?? int.MaxValue;
        return function.Invoke(Entities).Skip(pageSize * page.PageNumber).Take(takeSize);
    }

    public void ClearTrackedEntities() { _applicationDbContext.ChangeTracker.Clear(); }

    public async Task<T?> Get(TKey id) { return await Entities.FindAsync(id); }
    public T? Get(Func<DbSet<T>, T?> function) { return function.Invoke(Entities); }

    public async Task<T> Save(T entity)
    {
        var result = Entities.Add(entity);
        await _applicationDbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task Save(IEnumerable<T> entities)
    {
        Entities.AddRange(entities);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task<T> Update(T entity)
    {
        var result = Entities.Update(entity);
        await _applicationDbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task Update(IEnumerable<T> entities)
    {
        Entities.UpdateRange(entities);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async void Delete(TKey id)
    {
        var entity = await Entities.FindAsync(id);
        if (entity == null) return; // idempotency
        Entities.Remove(entity);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async void SaveChanges() { await _applicationDbContext.SaveChangesAsync(); }
}