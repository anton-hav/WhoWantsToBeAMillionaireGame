using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Core.Abstractions;

public interface IGameService
{
    Task<GameDto> GetNewGameDataAsync();
}