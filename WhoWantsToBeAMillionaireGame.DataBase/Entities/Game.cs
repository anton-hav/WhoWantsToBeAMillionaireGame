namespace WhoWantsToBeAMillionaireGame.DataBase.Entities;


// It entity will contain new property like ad datetime of creating,
// flags for a set of lifelines and etc.
// Datetime property need for adding cleaning database task to schedule like Hangfire.
public class Game : IBaseEntity
{
    public Guid Id { get; set; }
}