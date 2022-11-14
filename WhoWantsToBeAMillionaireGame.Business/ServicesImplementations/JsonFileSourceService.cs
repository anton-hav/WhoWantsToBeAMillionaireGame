﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WhoWantsToBeAMillionaireGame.Core;
using WhoWantsToBeAMillionaireGame.Core.Abstractions;
using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;
using WhoWantsToBeAMillionaireGame.Business.JsonConverters;
using System.ComponentModel.DataAnnotations;
using WhoWantsToBeAMillionaireGame.DataBase.Entities;

namespace WhoWantsToBeAMillionaireGame.Business.ServicesImplementations;

public class JsonFileSourceService : ISourceService
{
    private readonly string _sourceFilePath;
    private readonly JsonConverter _converter;

    public JsonFileSourceService(IConfiguration configuration)
    {
        _converter = new JsonToQuestionForDefaultSourceConverter();
        _sourceFilePath = configuration.GetSection("SourceFilePath")["Default"]
                          ?? throw new ArgumentNullException(nameof(_sourceFilePath));
    }

    public async Task<List<QuestionDto>?> GetQuestionsFromSourceAsync()
    {
        if (String.IsNullOrEmpty(_sourceFilePath))
            throw new ArgumentNullException($"Failed to get to {nameof(_sourceFilePath)} the sources file path from appseting.json.");

        var list = new List<QuestionDto>();
        var serializer = new JsonSerializer();

        await using var file = File.Open(_sourceFilePath, FileMode.Open);
        using var streamReader = new StreamReader(file);
        using JsonReader reader = new JsonTextReader(streamReader);
        while (await reader.ReadAsync())
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                var token = await JToken.LoadAsync(reader);
                var question = JsonConvert.DeserializeObject<QuestionDto>(token.ToString(), _converter);

                if (question != null) list.Add(question);
            }
        }

        //return list.GroupBy(dto => dto.Text).Select(dto => dto.First()).ToList();
        return ClearListOfDuplicateQuestionsAndInvalidAnswers(list);
    }

    /// <summary>
    /// It creates a new list without questions with a duplicate property Text and answers with a duplicate property Text.ToLower
    /// </summary>
    /// <param name="list"></param>
    /// <returns>a clean list without a duplicate</returns>
    private List<QuestionDto> ClearListOfDuplicateQuestionsAndInvalidAnswers(List<QuestionDto> list)
    {
        var pureList = list
            .Where(q => q.Answers
                .GroupBy(answer => answer.Text.ToLower())
                .SelectMany(grp => grp.Skip(1))
                .Count() == 0)
            .GroupBy(dto => dto.Text)
            .Select(dto => dto.First())
            .ToList();

        return pureList;
    } 


}