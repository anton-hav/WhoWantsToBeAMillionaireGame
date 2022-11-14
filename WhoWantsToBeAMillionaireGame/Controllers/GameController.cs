using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WhoWantsToBeAMillionaireGame.Core.Abstractions;

namespace WhoWantsToBeAMillionaireGame.Controllers
{
    public class GameController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;

        public GameController(IMapper mapper, 
            IQuestionService questionService)
        {
            _mapper = mapper;
            _questionService = questionService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _questionService.GetRandomizedPoolOfQuestionsForGame();
            return View();
        }
    }
}
