namespace WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

public class AnswerDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public Guid QuestionId { get; set; }
}