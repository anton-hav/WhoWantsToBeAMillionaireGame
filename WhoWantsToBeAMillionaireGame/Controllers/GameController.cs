using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WhoWantsToBeAMillionaireGame.Core.Abstractions;
using WhoWantsToBeAMillionaireGame.Extensions;
using WhoWantsToBeAMillionaireGame.Models;

namespace WhoWantsToBeAMillionaireGame.Controllers
{
    public class GameController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGameService _gameService;

        private const string SessionKeyGameId = "_GameId";

        public GameController(IMapper mapper, 
            IGameService gameService)
        {
            _mapper = mapper;
            _gameService = gameService;
        }

        public async Task<IActionResult> Index()
        {
            var gameId = HttpContext.Session.Get<Guid>(SessionKeyGameId);

            if (gameId.Equals(Guid.Empty))
            {
                gameId = Guid.NewGuid();
                HttpContext.Session.Set<Guid>(SessionKeyGameId, gameId);
                await _gameService.CreateNewGameAsync(gameId);
            }

            var dto = await _gameService.GetGameById(gameId);
            var model = _mapper.Map<GameModel>(dto);

            return View(model);
        }
    }
}
