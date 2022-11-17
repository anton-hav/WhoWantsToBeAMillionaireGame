using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Business.JsonConverters;

public class JsonToQuestionForDefaultSourceConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
    }

    public override QuestionDto? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;
        var token = JToken.Load(reader);

        var correctAnswer = token.Last?.First?.Value<string>();
        if (string.IsNullOrEmpty(correctAnswer))
            return null;

        var node = token.First;

        var questionText = node?.First?.Value<string>();
        if (string.IsNullOrEmpty(questionText))
            return null;

        var dto = new QuestionDto
        {
            Id = Guid.NewGuid(),
            Text = questionText,
            IsEnable = true
        };

        var answersList = new List<AnswerDto>();
        for (var i = 0; i < 4; i++)
        {
            node = node?.Next;

            if (node == null)
                return null;

            var textAnswer = node.First?.Value<string>();
            if (string.IsNullOrEmpty(textAnswer))
                return null;

            var isCorrect = node.Path.Equals(correctAnswer);

            var answer = new AnswerDto
            {
                Id = Guid.NewGuid(),
                Text = textAnswer,
                IsCorrect = isCorrect,
                QuestionId = dto.Id
            };

            answersList.Add(answer);
        }

        dto.Answers = answersList;

        return dto;
    }

    public override bool CanWrite => false;

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(QuestionDto);
    }
}