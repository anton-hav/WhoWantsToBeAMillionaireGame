﻿using WhoWantsToBeAMillionaireGame.Core.DataTransferObjects;

namespace WhoWantsToBeAMillionaireGame.Models;

public class GameModel
{
    public GameQuestionDto? GameQuestion { get; set; }
    public Guid UserChoice { get; set; }
}