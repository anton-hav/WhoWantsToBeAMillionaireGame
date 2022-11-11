using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WhoWantsToBeAMillionaireGame.Core.Abstractions;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;
using WhoWantsToBeAMillionaireGame.Data.Abstractions;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;

namespace WhoWantsToBeAMillionaireGame.Business.ServicesImplementations;

public class QuestionService : IQuestionService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public QuestionService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<QuestionDto> GetQuestionByIdAsync(Guid id)
    {
        throw new NotImplementedException();
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

    public async Task<int> UpdateAsync(Guid id, QuestionDto dto)
    {
        throw new NotImplementedException();
    }
}