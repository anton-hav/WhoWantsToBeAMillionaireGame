using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WhoWantsToBeAMillionaireGame.Core.Abstractions;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;
using WhoWantsToBeAMillionaireGame.Models;

namespace WhoWantsToBeAMillionaireGame.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;

        public QuestionController(IMapper mapper, 
            IQuestionService questionService)
        {
            _mapper = mapper;
            _questionService = questionService;
        }

        public IActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new QuestionModel();

                model.Answers = new List<AnswerDto>(4);

                for (var i = 1; i <= 4; i++)
                {
                    model.Answers.Add(new AnswerDto());
                }
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(QuestionModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Id = Guid.NewGuid();
                    var dto = _mapper.Map<QuestionDto>(model);
                    await _questionService.CreateQuestionAsync(dto);
                    return RedirectToAction("Index", "Question");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckQuestionForCreateOrEdit(string text, List<AnswerDto> answers, Guid id)
        {
            try
            {
                var isQuestionValid = await IsQuestionValidAsync(id, text);
                var isAnswersValid = IsAnswersValid(answers);

                return Ok(isQuestionValid && isAnswersValid);
            }
            catch (Exception e)
            {
                Log.Error($"{e.Message}. {Environment.NewLine} {e.StackTrace}");
                return StatusCode(500);
            }

            async Task<bool> IsQuestionValidAsync(Guid questionId, string questionText)
            {
                var isExist = await _questionService.IsQuestionExistAsync(questionText);

                if (!isExist) return true;

                if (!questionId.Equals(Guid.Empty))
                {
                    var isQuestionTheSame = await IsQuestionTheSameAsync(questionId, questionText);
                    if (isQuestionTheSame)
                    {
                        return true;
                    }
                }
                return false;
            }

            async Task<bool> IsQuestionTheSameAsync(Guid questionId, string questionText)
            {
                var dto = await _questionService.GetQuestionByIdAsync(questionId);
                return dto.Text.Equals(questionText);
            }

            bool IsAnswersValid(List<AnswerDto> answrs)
            {
                var result = answrs.GroupBy(answer => answer.Text)
                    .SelectMany(grp => grp.Skip(1)).Count();

                return result == 0;
            }
        }

    }
}
