using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Core.Abstractions;

public interface IAnswerService
{
    //READ
    Task<bool> IsAnswerCorrect(Guid id);

    //CREATE
    Task<int> CreateRangeOfAnswersAsync(List<AnswerDto> answers);

    //UPDATE

    //REMOVE
}