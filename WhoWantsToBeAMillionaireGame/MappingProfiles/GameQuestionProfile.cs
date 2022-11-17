using AutoMapper;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;

namespace WhoWantsToBeAMillionaireGame.MappingProfiles;

public class GameQuestionProfile : Profile
{
    public GameQuestionProfile()
    {
        CreateMap<GameQuestion, GameQuestionDto>();
        CreateMap<GameQuestionDto, GameQuestion>();
    }
}