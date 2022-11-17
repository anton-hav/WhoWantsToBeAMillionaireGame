namespace WhoWantsToBeAMillionaireGame.DataBase.Entities;

public class Answer : IBaseEntity
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public Guid QuestionId { get; set; }
}