using WhoWantsToBeAMillionaireGame.DataBase.Entities;

namespace WhoWantsToBeAMillionaireGame.Data.Abstractions.Repositories;

public interface IRepository<T> where T : IBaseEntity
{
    //READ
    Task<T?> GetByIdAsync(Guid id);
    IQueryable<T> Get();
    
    //CREATE
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

    //UPDATE
    void Update(T entity);

    //DELETE
    void Remove(T entity);

}