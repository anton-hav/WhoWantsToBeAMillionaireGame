using AutoMapper;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;
using WhoWantsToBeAMillionaireGame.Models;

namespace WhoWantsToBeAMillionaireGame.MappingProfiles;

public class GameProfile : Profile
{
    public GameProfile()
    {
        CreateMap<Game, GameDto>();
        CreateMap<GameDto, Game>();

        CreateMap<GameDto, GameModel>();
        CreateMap<GameModel, GameDto>();
    }
}