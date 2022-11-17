namespace WhoWantsToBeAMillionaireGame.DataBase.Entities;

public class GameQuestion : IBaseEntity
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }
    public Question Question { get; set; }

    public Guid GameId { get; set; }

    public bool IsSuccessful { get; set; }
}