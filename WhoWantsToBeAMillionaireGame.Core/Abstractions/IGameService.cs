using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Core.Abstractions;

public interface IGameService
{
    // READ
    Task<bool> IsUnfinishedGameExistById(Guid id);
    Task<GameDto> GetGameById(Guid id);

    // CREATE
    Task<int> CreateNewGameAsync(Guid id);

    // UPDATE
}