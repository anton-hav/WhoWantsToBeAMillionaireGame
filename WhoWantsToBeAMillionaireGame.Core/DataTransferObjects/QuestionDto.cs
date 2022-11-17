namespace WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

public class QuestionDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public bool IsEnable { get; set; }

    public List<AnswerDto> Answers { get; set; }
}