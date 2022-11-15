using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WhoWantsToBeAMillionaireGame.Core.Abstractions;
using WhoWantsToBeAMillionaireGame.Models;

namespace WhoWantsToBeAMillionaireGame.Controllers
{
    public class GameController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IGameService _gameService;

        public GameController(IMapper mapper, 
            IGameService gameService)
        {
            _mapper = mapper;
            _gameService = gameService;
        }

        public async Task<IActionResult> Index()
        {
            HttpContext.Session.SetString("GameId", Guid.NewGuid().ToString("D"));

            var dto = await _gameService.GetNewGameDataAsync();
            //todo: if the GameModel is the same GameDto remove mapping and use GameDto directly
            var model = _mapper.Map<GameModel>(dto);
            return View(model);
        }
    }
}
