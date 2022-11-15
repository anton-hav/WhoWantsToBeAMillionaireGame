using AutoMapper;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;
using WhoWantsToBeAMillionaireGame.Models;

namespace WhoWantsToBeAMillionaireGame.MappingProfiles;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<GameDto, GameModel>();
        CreateMap<GameModel, GameDto>();
    }
}

