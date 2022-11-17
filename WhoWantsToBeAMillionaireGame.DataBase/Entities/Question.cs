namespace WhoWantsToBeAMillionaireGame.DataBase.Entities;

public class Question : IBaseEntity
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsEnable { get; set; }

    public List<Answer> Answers { get; set; }
}