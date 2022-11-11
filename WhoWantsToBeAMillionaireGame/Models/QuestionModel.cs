using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Models;

public class QuestionModel
{
    public Guid Id { get; set; }
    [Required]
    [Remote("CheckQuestionForCreateOrEdit", "Question",
        AdditionalFields = nameof(Answers) 
                           + "," + nameof(Id),
        HttpMethod = WebRequestMethods.Http.Post,
        ErrorMessage = "The question with the same text already exist.")]
    public string Text { get; set; }

    [Required]
    public List<AnswerDto> Answers { get; set; }
}