using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Core.Abstractions;

public interface IAnswerService
{
    //READ
    Task<AnswerDto> GetAnswerByIdAsync(Guid id);
    Task<bool> IsAnswerExistAsync(string text);
    Task<bool> IsAnswerCorrect(Guid id);

    //CREATE
    Task<int> CreateAnswerAsync(AnswerDto dto);
    Task<int> CreateRangeOfAnswersAsync(List<AnswerDto> answers);

    //UPDATE
    Task<int> UpdateAsync(Guid id, AnswerDto dto);

    //REMOVE

}