using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Core.Abstractions;

public interface IGameService
{
    // READ
    Task<GameDto> GetGameById(Guid id);
    Task<bool> IsAnswerCorrect(Guid answerId);
    Guid GetIdForCorrectAnswerOfCurrentQuestionByGameIdAsync(Guid gameId);
    GameQuestionDto GetCurrentQuestionByGameIdAsync(Guid gameId);

    // CREATE
    Task<int> CreateNewGameAsync(Guid id);

    // UPDATE
    Task<int> MarkCurrentGameQuestionAsSuccessful(Guid gameId);
}