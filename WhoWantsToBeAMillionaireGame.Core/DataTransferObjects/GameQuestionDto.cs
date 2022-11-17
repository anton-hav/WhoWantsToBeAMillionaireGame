namespace WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

public class GameQuestionDto
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }
    public QuestionDto Question { get; set; }

    public Guid GameId { get; set; }

    public bool IsSuccessful { get; set; }
}