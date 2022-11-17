using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WhoWantsToBeAMillionaireGame.Core;
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
    private readonly IAnswerService _answerService;

    public GameService(IMapper mapper,
        IQuestionService questionService,
        IUnitOfWork unitOfWork,
        IAnswerService answerService)
    {
        _mapper = mapper;
        _questionService = questionService;
        _unitOfWork = unitOfWork;
        _answerService = answerService;
    }

    public async Task<GameDto> GetGameById(Guid id)
    {
        var game = await _unitOfWork.Game.GetByIdAsync(id);
        if (game == null) throw new ArgumentException(nameof(id));

        var dto = _mapper.Map<GameDto>(game);
        var gameQuestion = _unitOfWork.GameQuestion
            .Get()
            .Where(gq => gq.GameId.Equals(id) && gq.IsSuccessful.Equals(false))
            .OrderBy(gq => gq.Id)
            .Include(gq => gq.Question.Answers)
            .AsNoTracking()
            .FirstOrDefault();

        dto.GameQuestion = _mapper.Map<GameQuestionDto>(gameQuestion);

        return dto;
    }

    public async Task<bool> IsAnswerCorrect(Guid answerId)
    {
        var isCorrect = await _answerService.IsAnswerCorrect(answerId);

        return isCorrect;
    }

    public Guid GetIdForCorrectAnswerOfCurrentQuestionByGameIdAsync(Guid gameId)
    {
        var correctAnswer = _unitOfWork.GameQuestion
            .Get()
            .Where(gq => gq.GameId.Equals(gameId) && gq.IsSuccessful.Equals(false))
            .OrderBy(gq => gq.Id)
            .Include(gq => gq.Question.Answers.Where(answer => answer.IsCorrect.Equals(true)))
            .AsNoTracking()
            .FirstOrDefault();

        if (correctAnswer == null) throw new ArgumentException("Failed to find the correct answer in the database.");

        return correctAnswer.Question.Answers.First().Id;
    }

    public GameQuestionDto GetCurrentQuestionByGameIdAsync(Guid gameId)
    {
        var gameQuestion = _unitOfWork.GameQuestion
            .Get()
            .Where(gq => gq.GameId.Equals(gameId) && gq.IsSuccessful.Equals(false))
            .OrderBy(gq => gq.Id)
            .Include(gq => gq.Question.Answers)
            .AsNoTracking()
            .FirstOrDefault();

        if (gameQuestion == null)
            throw new ArgumentException("Failed to get the game question for the game in the database.");
        var dto = _mapper.Map<GameQuestionDto>(gameQuestion);

        return dto;
    }

    public async Task<int> CreateNewGameAsync(Guid id)
    {
        var game = new Game
        {
            Id = id
        };

        var questions = await _questionService.GetRandomizedPoolOfQuestionsForGame();

        var addDbContextTasks = new List<Task>
        {
            _unitOfWork.Game.AddAsync(game)
        };
        var gameQuestions = questions
            .Select(q => new GameQuestion
            {
                Id = Guid.NewGuid(),
                GameId = game.Id,
                IsSuccessful = false,
                QuestionId = q.Id
            });

        addDbContextTasks.Add(_unitOfWork.GameQuestion.AddRangeAsync(gameQuestions));


        await Task.WhenAll(addDbContextTasks);

        return await _unitOfWork.Commit();
    }

    public async Task<int> MarkCurrentGameQuestionAsSuccessful(Guid gameId)
    {
        var gameQuestion = _unitOfWork.GameQuestion
            .Get()
            .Where(gq => gq.GameId.Equals(gameId) && gq.IsSuccessful.Equals(false))
            .OrderBy(gq => gq.Id)
            .FirstOrDefault();

        var patchList = new List<PatchModel>();

        if (gameQuestion is { IsSuccessful: false })
            patchList.Add(new PatchModel
            {
                PropertyName = nameof(gameQuestion.IsSuccessful),
                PropertyValue = true
            });
        if (gameQuestion != null)
            await _unitOfWork.GameQuestion.PatchAsync(gameQuestion.Id, patchList);
        return await _unitOfWork.Commit();
    }
}