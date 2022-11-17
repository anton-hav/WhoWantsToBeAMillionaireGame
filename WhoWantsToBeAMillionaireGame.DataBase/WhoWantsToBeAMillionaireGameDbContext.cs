using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Numerics;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;

namespace WhoWantsToBeAMillionaireGame.DataBase;

public class WhoWantsToBeAMillionaireGameDbContext : DbContext
{
    public DbSet<Question> Question { get; set; }
    public DbSet<Answer> Answer { get; set; }
    public DbSet<Game> Game { get; set; }
    public DbSet<GameQuestion> GameQuestions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Question>()
            .HasIndex(question => question.Text)
            .IsUnique();

        builder.Entity<Answer>()
            .HasIndex(answer => new {
                answer.Text,
                answer.QuestionId
            })
        .IsUnique();

        builder.Entity<GameQuestion>()
            .HasIndex(gameQuestion => new
            {
                gameQuestion.GameId,
                gameQuestion.QuestionId
            });

        base.OnModelCreating(builder);
    }

    public WhoWantsToBeAMillionaireGameDbContext(DbContextOptions<WhoWantsToBeAMillionaireGameDbContext> options)
        : base(options)
    {
    }
}