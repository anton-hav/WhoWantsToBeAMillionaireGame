using AutoMapper;
using WhoWantsToBeAMillionaireGame.Core.Abstractions;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;
using WhoWantsToBeAMillionaireGame.Data.Abstractions;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;

namespace WhoWantsToBeAMillionaireGame.Business.ServicesImplementations;

public class AnswerService : IAnswerService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AnswerService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<AnswerDto> GetAnswerByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsAnswerExistAsync(string text)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CreateAnswerAsync(AnswerDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<int> CreateRangeOfAnswersAsync(List<AnswerDto> answers)
    {
        var entities = _mapper.Map<List<Answer>>(answers);
        await _unitOfWork.Answer.AddRangeAsync(entities);
        var result = await _unitOfWork.Commit();
        return result;
    }

    public async Task<int> UpdateAsync(Guid id, AnswerDto dto)
    {
        throw new NotImplementedException();
    }
}