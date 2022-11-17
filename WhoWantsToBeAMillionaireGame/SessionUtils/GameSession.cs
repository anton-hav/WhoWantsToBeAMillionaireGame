namespace WhoWantsToBeAMillionaireGame.SessionUtils;

public class GameSession
{
    public Guid GameId { get; set; }
    public Guid UserChoiceId { get; set; }
    public int QuestionNumber { get; set; }
    public bool IsTookMoney { get; set; }
}