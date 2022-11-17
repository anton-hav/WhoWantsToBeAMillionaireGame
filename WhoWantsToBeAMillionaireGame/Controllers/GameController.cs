using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using WhoWantsToBeAMillionaireGame.Core.Abstractions;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;
using WhoWantsToBeAMillionaireGame.Models;
using WhoWantsToBeAMillionaireGame.SessionUtils;

namespace WhoWantsToBeAMillionaireGame.Controllers
{
    public class GameController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGameService _gameService;

        private const string GameSessionKey = "_Game";

        public GameController(IMapper mapper,
            IGameService gameService)
        {
            _mapper = mapper;
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {

                var isSucceed = HttpContext.Session.TryGetValue<GameSession>(GameSessionKey, out var gameSession);

                //todo: add a middle page with information about the end of the waiting time
                if (!isSucceed)
                {
                    gameSession = new GameSession()
                    {
                        GameId = Guid.NewGuid(),
                        UserChoiceId = Guid.Empty,
                        QuestionNumber = 1,
                    };
                    HttpContext.Session.Set<GameSession>(GameSessionKey, gameSession);
                    await _gameService.CreateNewGameAsync(gameSession.GameId);
                }

                var dto = await _gameService.GetGameById(gameSession.GameId);
                var model = _mapper.Map<GameModel>(dto);
                model.UserChoice = gameSession.UserChoiceId;

                return View(model);
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> IsUserChoiceCorrect([FromBody]Guid userChoice)
        {
            try
            {
                var gameSession = HttpContext.Session.Get<GameSession>(GameSessionKey);
                if (gameSession != null)
                {
                    gameSession.UserChoiceId = userChoice;
                    HttpContext.Session.Set<GameSession>(GameSessionKey, gameSession);
                }

                var isCorrect = await _gameService.IsAnswerCorrect(userChoice);
                return Ok(isCorrect);
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult GetCorrectAnswerId()
        {
            try
            {
                
                var isSucceed = HttpContext.Session.TryGetValue<GameSession>(GameSessionKey, out var gameSession);

                //todo: add a middle page with information about the end of the waiting time
                if (!isSucceed)
                    RedirectToAction("Index", "Home");

                var correctAnswerId = _gameService.GetIdForCorrectAnswerOfCurrentQuestionByGameIdAsync(gameSession.GameId);
                return Ok(correctAnswerId);
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult GameOver()
        {
            try
            {
                HttpContext.Session.Remove(GameSessionKey);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> MarkGameCurrentQuestionAsSuccessful()
        {
            try
            {
                var isSucceed = HttpContext.Session.TryGetValue<GameSession>(GameSessionKey, out var gameSession);

                //todo: add a middle page with information about the end of the waiting time
                if (!isSucceed)
                    RedirectToAction("Index", "Home");
                
                var result = await _gameService.MarkCurrentGameQuestionAsSuccessful(gameSession.GameId);
                return Ok(result != 0);
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentQuestionNumber()
        {
            try
            {
                var isSucceed = HttpContext.Session.TryGetValue<GameSession>(GameSessionKey, out var gameSession);

                //todo: add a middle page with information about the end of the waiting time
                if (!isSucceed)
                    RedirectToAction("Index", "Home");

                return Ok(gameSession.QuestionNumber);
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult GetNextGameQuestion()
        {
            try
            {
                var isSucceed = HttpContext.Session.TryGetValue<GameSession>(GameSessionKey, out var gameSession);

                //todo: add a middle page with information about the end of the waiting time
                if (!isSucceed)
                    RedirectToAction("Index", "Home");
                
                var gameQuestion = _gameService.GetCurrentQuestionByGameIdAsync(gameSession.GameId);

                gameSession.UserChoiceId = Guid.Empty;
                gameSession.QuestionNumber++;

                HttpContext.Session.Set<GameSession>(GameSessionKey, gameSession);

                return Ok(gameQuestion);
            }
            catch (ArgumentException ex)
            {
                var questionNumber = GetQuestionNumberFromSession();
                if (questionNumber.Equals(15))
                {
                    return Ok();
                }

                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        private int GetQuestionNumberFromSession()
        {
            var isSucceed = HttpContext.Session.TryGetValue<GameSession>(GameSessionKey, out var gameSession);

            //todo: add a middle page with information about the end of the waiting time
            if (!isSucceed)
                throw new ArgumentException("Failed to get value from session find session.");

            return gameSession.QuestionNumber;
        }
    }
}