using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WhoWantsToBeAMillionaireGame.Core;
using WhoWantsToBeAMillionaireGame.Core.Abstractions;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;
using WhoWantsToBeAMillionaireGame.Data.Abstractions;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;

namespace WhoWantsToBeAMillionaireGame.Business.ServicesImplementations;

public class QuestionService : IQuestionService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAnswerService _answerService;
    private readonly ISourceService _sourceService;

    public QuestionService(IMapper mapper,
        IUnitOfWork unitOfWork,
        IAnswerService answerService,
        ISourceService sourceService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _answerService = answerService;
        _sourceService = sourceService;
    }

    public async Task<QuestionDto> GetQuestionByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Question.GetByIdAsync(id);

        if (entity != null)
        {
            var dto = _mapper.Map<QuestionDto>(entity);
            return dto;
        }
        else
        {
            throw new ArgumentException(nameof(id));
        }
    }

    // todo: implement it with pagination page because GetAll is a bad way
    public async Task<List<QuestionDto>> GetAllQuestionAsync()
    {
        return await _unitOfWork.Question
            .Get()
            .AsNoTracking()
            .Select(question => _mapper.Map<QuestionDto>(question))
            .ToListAsync();
    }

    public async Task<bool> IsQuestionExistAsync(string text)
    {
        var entity = await _unitOfWork.Question
            .Get()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Text.Equals(text));
        return entity != null;
    }

    public async Task<int> CreateQuestionAsync(QuestionDto dto)
    {
        var entity = _mapper.Map<Question>(dto);

        if (entity == null)
            throw new ArgumentException("Mapping QuestionDto to Question was not possible.", nameof(dto));

        await _unitOfWork.Question.AddAsync(entity);

        var result = await _unitOfWork.Commit();
        return result;
    }

    public async Task<int> AggregateQuestionsFromExternalSourceAsync()
    {
        var list = await _sourceService.GetQuestionsFromSourceAsync();
        if (list.IsNullOrEmpty()) return 0;

        var entities = new List<Question>();
        foreach (var dto in list)
        {
            var isExist = await IsQuestionExistAsync(dto.Text);
            if (!isExist)
            {
                entities.Add(_mapper.Map<Question>(dto));
            }
        }

        await _unitOfWork.Question.AddRangeAsync(entities);

        return await _unitOfWork.Commit();
    }

    public async Task<int> UpdateAsync(Guid id, QuestionDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<int> ChangeAvailabilityAsync(Guid id, bool newValue)
    {
        var dto = await GetQuestionByIdAsync(id);
        var patchList = new List<PatchModel>();
        if (dto != null)
        {
            if (!dto.IsEnable.Equals(newValue))
            {
                patchList.Add(new PatchModel() { PropertyName = nameof(dto.IsEnable), PropertyValue = newValue });
            }
        }

        await _unitOfWork.Question.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var entity = await _unitOfWork.Question
            .Get()
            .FirstOrDefaultAsync(entity => entity.Id.Equals(id));

        if (entity != null)
        {
            _unitOfWork.Question.Remove(entity);
            return await _unitOfWork.Commit();
        }
        else
        {
            throw new ArgumentException(nameof(id));
        }
    }
}