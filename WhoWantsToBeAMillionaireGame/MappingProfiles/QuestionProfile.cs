using AutoMapper;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;
using WhoWantsToBeAMillionaireGame.Models;

namespace WhoWantsToBeAMillionaireGame.MappingProfiles;

public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        CreateMap<Question, QuestionDto>();
        CreateMap<QuestionDto, Question>();

        CreateMap<QuestionDto, QuestionModel>();
        CreateMap<QuestionModel, QuestionDto>();
    }
}