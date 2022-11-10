using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WhoWantsToBeAMillionaireGame.Core;
using WhoWantsToBeAMillionaireGame.Data.Abstractions;
using WhoWantsToBeAMillionaireGame.Data.Abstractions.Repositories;
using WhoWantsToBeAMillionaireGame.DataBase;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;

namespace WhoWantsToBeAMillionaireGame.Data.Repositories;

public class Repository<T> : IRepository<T>
    where T : class, IBaseEntity
{
    protected readonly WhoWantsToBeAMillionaireGameDbContext DbContext;
    protected readonly DbSet<T> DbSet;

    public Repository(WhoWantsToBeAMillionaireGameDbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = dbContext.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id.Equals(id));
    }

    public IQueryable<T> Get()
    {
        return DbSet;
    }

    public virtual async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public virtual void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public virtual void Remove(T entity)
    {
        DbSet.Remove(entity);
    }
}