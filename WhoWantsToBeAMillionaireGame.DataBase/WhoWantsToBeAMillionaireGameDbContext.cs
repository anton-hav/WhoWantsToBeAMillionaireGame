using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Numerics;
using System.Reflection.PortableExecutable;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;

namespace WhoWantsToBeAMillionaireGame.DataBase;

public class WhoWantsToBeAMillionaireGameDbContext : DbContext
{
    public DbSet<Question> Question { get; set; }
    public DbSet<Answer> Answer { get; set; }

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

        // Disable cascade deletion
        // Note by Anton: Cascade deletion is not bad for this project,
        // but I prefer to control the deletion process
        var cascadeFKs = builder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership
                         && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
            fk.DeleteBehavior = DeleteBehavior.Restrict;

        base.OnModelCreating(builder);
    }

    public WhoWantsToBeAMillionaireGameDbContext(DbContextOptions<WhoWantsToBeAMillionaireGameDbContext> options)
        : base(options)
    {
    }
}