using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Core.Abstractions;

public interface IQuestionService
{
    //READ
    Task<QuestionDto> GetQuestionByIdAsync(Guid id);
    Task<List<QuestionDto>> GetAllQuestionAsync();
    Task<bool> IsQuestionExistAsync(string text);
    Task<List<QuestionDto>> GetRandomizedPoolOfQuestionsForGame();

    //CREATE
    Task<int> CreateQuestionAsync(QuestionDto dto);
    Task<int> AggregateQuestionsFromExternalSourceAsync();

    //UPDATE
    Task<int> ChangeAvailabilityAsync(Guid id, bool newValue);

    //REMOVE
    Task<int> DeleteAsync(Guid id);

}