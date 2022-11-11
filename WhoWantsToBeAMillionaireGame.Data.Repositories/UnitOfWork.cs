using Microsoft.EntityFrameworkCore;
using WhoWantsToBeAMillionaireGame.Data.Abstractions;
using WhoWantsToBeAMillionaireGame.Data.Abstractions.Repositories;
using WhoWantsToBeAMillionaireGame.DataBase;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;

namespace WhoWantsToBeAMillionaireGame.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly WhoWantsToBeAMillionaireGameDbContext _dbContext;
    public IRepository<Question> Question { get; }
    public IRepository<Answer> Answer { get; }

    public UnitOfWork(WhoWantsToBeAMillionaireGameDbContext dbContext, 
        IRepository<Question> question, 
        IRepository<Answer> answer)
    {
        _dbContext = dbContext;
        Question = question;
        Answer = answer;
    }
    
    public async Task<int> Commit()
    {
        return await _dbContext.SaveChangesAsync();
    }
}