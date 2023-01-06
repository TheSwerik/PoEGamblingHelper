﻿using Microsoft.EntityFrameworkCore;
using Model;
using Model.QueryParameters;

namespace Backend.Data;

public interface IRepository<T, TKey> where T : class, IEntity //TODO fix this shit
{
    IAsyncEnumerable<T> GetAllAsync();
    IEnumerable<T> GetAll();
    Task<T?> Get(TKey id);
    T? Get(Func<DbSet<T>, T?> function);
    Task<T> Save(T entity);
    Task Save(IEnumerable<T> entity);
    Task<T> Update(T entity);
    Task Update(IEnumerable<T> entity);
    void Delete(TKey id);
    void SaveChanges();
    IEnumerable<T> GetAll(Func<DbSet<T>, IEnumerable<T>> function);
    IEnumerable<T> GetAll(Page? page, Func<DbSet<T>, IEnumerable<T>> function);
    IAsyncEnumerable<T> GetAllAsync(Func<DbSet<T>, IQueryable<T>> function);
    IAsyncEnumerable<T> GetAllAsync(Page? page, Func<DbSet<T>, IQueryable<T>> function);
    void ClearTrackedEntities();
}