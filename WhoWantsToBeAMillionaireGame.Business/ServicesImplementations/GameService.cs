using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using WhoWantsToBeAMillionaireGame.Core.Abstractions;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Business.ServicesImplementations;

public class GameService : IGameService
{
    private readonly IMapper _mapper;
    private readonly IQuestionService _questionService;

    public GameService(IMapper mapper, 
        IQuestionService questionService)
    {
        _mapper = mapper;
        _questionService = questionService;
    }

    public async Task<GameDto> CreateNewGameAsync(Guid id)
    {
        var questions = await _questionService.GetRandomizedPoolOfQuestionsForGame();

        var game = new GameDto();

        return game;

    }
}