using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Core.Abstractions;

public interface IQuestionService
{
    //READ
    Task<QuestionDto> GetQuestionByIdAsync(Guid id);
    Task<List<QuestionDto>> GetAllQuestionAsync();
    Task<bool> IsQuestionExistAsync(string text);

    //CREATE
    Task<int> CreateQuestionAsync(QuestionDto dto);
    Task<int> AggregateQuestionsFromExternalSourceAsync();

    //UPDATE
    Task<int> UpdateAsync(Guid id, QuestionDto dto);
    Task<int> ChangeAvailabilityAsync(Guid id, bool newValue);

    //REMOVE
    Task<int> DeleteAsync(Guid id);

}