using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Core.Abstractions;

public interface IQuestionService
{
    //READ
    Task<QuestionDto> GetQuestionByIdAsync(Guid id);
    Task<bool> IsQuestionExistAsync(string text);

    //CREATE
    Task<int> CreateQuestionAsync(QuestionDto dto);

    //UPDATE
    Task<int> UpdateAsync(Guid id, QuestionDto dto);

}