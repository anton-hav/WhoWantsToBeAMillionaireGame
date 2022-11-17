using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Core.Abstractions;

public interface ISourceService
{
    Task<List<QuestionDto>?> GetQuestionsFromSourceAsync();
}