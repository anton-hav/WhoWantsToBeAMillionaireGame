namespace WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

public class GameDto
{
    public Guid Id { get; set; }
    public GameQuestionDto? GameQuestion { get; set; }
}