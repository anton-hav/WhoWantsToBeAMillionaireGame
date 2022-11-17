using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Models;

public class QuestionModel : IValidatableObject
{
    public Guid Id { get; set; }

    [Required]
    [Remote("CheckQuestionForCreateOrEdit", "Question",
        AdditionalFields = nameof(Id),
        HttpMethod = WebRequestMethods.Http.Post,
        ErrorMessage = "The question with the same text already exist.")]
    public string Text { get; set; }

    public bool IsEnable { get; set; }

    [Required] public List<AnswerDto> Answers { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var result = Answers.GroupBy(answer => answer.Text)
            .SelectMany(grp => grp.Skip(1)).Count();
        if (result != 0)
            yield return new ValidationResult("Several of the answers are identical. Each answer must be unique.");
    }
}