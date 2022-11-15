using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WhoWantsToBeAMillionaireGame.Core.Abstractions;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;
using WhoWantsToBeAMillionaireGame.Data.Abstractions;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;

namespace WhoWantsToBeAMillionaireGame.Business.ServicesImplementations;

public class GameService : IGameService
{
    private readonly IMapper _mapper;
    private readonly IQuestionService _questionService;
    private readonly IUnitOfWork _unitOfWork;

    public GameService(IMapper mapper, 
        IQuestionService questionService, 
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _questionService = questionService;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<bool> IsUnfinishedGameExistById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<GameDto> GetGameById(Guid id)
    {
        var getGameQuestionTask = new Task<GameQuestion?>(() => _unitOfWork.GameQuestion
            .Get()
            .Where(gq => gq.GameId.Equals(id) && gq.IsSuccessful.Equals(false))
            .OrderBy(gq => gq.Id)
            .Include(gq => gq.Question.Answers)
            .AsNoTracking()
            .FirstOrDefault());

        getGameQuestionTask.Start();

        var game = await _unitOfWork.Game.GetByIdAsync(id);
        if (game == null) throw new ArgumentException(nameof(id));

        var dto = _mapper.Map<GameDto>(game);
        var gameQuestion = await getGameQuestionTask;
        dto.GameQuestion = _mapper.Map<GameQuestionDto>(gameQuestion);

        return dto;
    }

    public async Task<int> CreateNewGameAsync(Guid id)
    {
        var game = new Game()
        {
            Id = id
        };

        var questions = await _questionService.GetRandomizedPoolOfQuestionsForGame();

        var addDbContextTasks = new List<Task>
        {
            _unitOfWork.Game.AddAsync(game)
        };
        var gameQuestions = questions
            .Select(q => new GameQuestion()
            {
                Id = Guid.NewGuid(),
                GameId = game.Id,
                IsSuccessful = false,
                QuestionId = q.Id,
            });

        addDbContextTasks.Add(_unitOfWork.GameQuestion.AddRangeAsync(gameQuestions));


        await Task.WhenAll(addDbContextTasks);
        
        return await _unitOfWork.Commit();

    }
}