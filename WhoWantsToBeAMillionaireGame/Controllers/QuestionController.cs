using System.Runtime.CompilerServices;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

        public async Task<IActionResult> Index()
        {
            try
            {
                var dto = await _questionService.GetAllQuestionAsync();
                var model = _mapper.Map<List<QuestionModel>>(dto);
                return View(model);
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
                var model = new QuestionModel()
                {
                    Id = Guid.NewGuid(),
                    IsEnable = true
                };

                model.Answers = new List<AnswerDto>
                {
                    new AnswerDto() { Id = Guid.NewGuid(), QuestionId = model.Id, IsCorrect = true },
                    new AnswerDto() { Id = Guid.NewGuid(), QuestionId = model.Id, IsCorrect = false },
                    new AnswerDto() { Id = Guid.NewGuid(), QuestionId = model.Id, IsCorrect = false },
                    new AnswerDto() { Id = Guid.NewGuid(), QuestionId = model.Id, IsCorrect = false },
                };


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
        public async Task<IActionResult> CheckQuestionForCreateOrEdit(string text, Guid id)
        {
            try
            {

                var isQuestionValid = await IsQuestionValidAsync(id, text);
                if (!isQuestionValid)
                    return Ok(false);

                return Ok(true);
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
        }

        [HttpGet]
        public async Task<IActionResult> Enable(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentException(nameof(id));
                }

                await _questionService.ChangeAvailabilityAsync(id, true);

                return RedirectToAction("Index", "Question");
            }
            catch (ArgumentException ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Disable(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentException(nameof(id));
                }

                await _questionService.ChangeAvailabilityAsync(id, false);

                return RedirectToAction("Index", "Question");
            }
            catch (ArgumentException ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentException(nameof(id));
                }
                
                await _questionService.DeleteAsync(id);

                return RedirectToAction("Index", "Question");
            }
            catch (ArgumentException ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Error($"{ex.Message}. {Environment.NewLine} {ex.StackTrace}");
                return StatusCode(500);
            }
        }

    }
}
