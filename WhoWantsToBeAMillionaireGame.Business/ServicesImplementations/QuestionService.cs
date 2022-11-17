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
    private readonly ISourceService _sourceService;

    public QuestionService(IMapper mapper,
        IUnitOfWork unitOfWork,
        ISourceService sourceService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
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

        throw new ArgumentException(nameof(id));
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

    public async Task<List<QuestionDto>> GetRandomizedPoolOfQuestionsForGame()
    {
        var list = await _unitOfWork.Question
            .Get()
            .Where(question => question.IsEnable.Equals(true))
            .OrderBy(question => Guid.NewGuid())
            .Take(15)
            .Include(question => question.Answers)
            .AsNoTracking()
            .Select(question => _mapper.Map<QuestionDto>(question))
            .ToListAsync();

        if (list.IsNullOrEmpty())
            throw new Exception("Database don't contains any question. Please add questions to database");

        return list;
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

        if (list != null)
            foreach (var dto in list)
            {
                var isExist = await IsQuestionExistAsync(dto.Text);
                if (!isExist) entities.Add(_mapper.Map<Question>(dto));
            }

        await _unitOfWork.Question.AddRangeAsync(entities);

        return await _unitOfWork.Commit();
    }

    public async Task<int> ChangeAvailabilityAsync(Guid id, bool newValue)
    {
        var dto = await GetQuestionByIdAsync(id);
        var patchList = new List<PatchModel>();
        if (!dto.IsEnable.Equals(newValue))
            patchList.Add(new PatchModel { PropertyName = nameof(dto.IsEnable), PropertyValue = newValue });

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

        throw new ArgumentException(nameof(id));
    }
}