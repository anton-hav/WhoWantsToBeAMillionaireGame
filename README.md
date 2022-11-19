# WhoWantsToBeAMillionaireGame
The project implements a simplified version of Who Wants to Be a Millionaire

## About
This project implements a version of Who Wants to Be a Millionaire using ASP.NET Core MVC.

### Main application features
- the game,
- adding new questions by the user,
- loading a list of questions from a JSON file (read more about the structure of the file below),
- managing the availability of questions for the game,
- remove questions from the database.

### Rules of the game

The game on "Who Wants to Be a Millionaire" is a series of multiple-choice questions that each have four possible answers (A,B,C,D). The contestant must answer 15 of these questions correctly, one at a time, in order to win the million złoty. As soon as the contestant answers a question incorrectly, the game is over.

During their game, the player has two "safety nets" – if a contestant gets a question wrong, but had reached a designated cash value during their game, they will leave with that amount as their prize. If a contestant feels unsure about an answer and does not wish to play on, they can walk away with the money they have won.

## Application preview
This part contains some  .gif files that show how the application work.

<details>
  <summary>Manage questions</summary>
  <a href="https://ibb.co/yqP6yYZ"><img src="https://i.ibb.co/5L4r8nS/manage-questions.gif" alt="manage-questions" border="0"></a>
</details>

<details>
  <summary>Load question from the source file</summary>
  <a href="https://ibb.co/Snx09sb"><img src="https://i.ibb.co/C7tvT1g/load-question-from-source.gif" alt="load-question-from-source" border="0"></a>
</details>

<details>
  <summary>Add new question</summary>
  <a href="https://ibb.co/RgVBmrn"><img src="https://i.ibb.co/9pCVLKX/add-new-question.gif" alt="add-new-question" border="0"></a>
</details>

<details>
  <summary>Win preview</summary>
  <a href="https://ibb.co/myVqZX2"><img src="https://i.ibb.co/R6k0rBs/win-preview.gif" alt="win-preview" border="0"></a>
</details>

<details>
  <summary>Unborning level preview</summary>
  <a href="https://ibb.co/cYgg818"><img src="https://i.ibb.co/ph00KxK/unburning-level-preview.gif" alt="unburning-level-preview" border="0"></a>
</details>

<details>
  <summary>Gameplay preview</summary>
  <a href="https://ibb.co/PFnKFk8"><img src="https://i.ibb.co/34q24KH/game-preview.gif" alt="game-preview" border="0"></a>
</details>

<details>
  <summary>Session at work</summary>
  <a href="https://ibb.co/PNg64X1"><img src="https://i.ibb.co/BtBCjWT/session-atwork-preview.gif" alt="session-atwork-preview" border="0"></a>
</details>



## How to configurate
### 1. Change database connection string
Firstly, you should change connection string in `appsettings.json`

```json
"ConnectionStrings": {
    "Default": "Server=myServer;Database=myDataBase;User Id=myUsername;Password=myPassword;TrustServerCertificate=True"
  },
```

If you run application in dev environment, change connection string in `appsettings.Development.json`

```json
  "ConnectionStrings": {
    "Default": "Server=YOUR-SERVER-NAME;Database=WhoWantsToBeAMillionaireGameDataBase;Trusted_Connection=True;TrustServerCertificate=True"
  },
```

>**Note**
> The project is designed to use Microsoft SQL.Server version 15 or higher. For versions below 15 stable operations is not guaranteed. The project uses the GUID type as the primary and therefore using other database providers requires additional changes and adjustments.

#### 2. Initiate database

Open the Packege Manager Console in Visual Studio (VIew -> Other Windows -> Packege Manager Console). Choose WhoWantsToBeAMillonaireGame.DataBase in Default project.

<a href="https://ibb.co/NjvGv6q"><img src="https://i.ibb.co/G26r6vD/Default-project-prt-sc.png" alt="Default-project-prt-sc" border="0"></a>

Use the following command to create or update the database schema. 

```console
PM> update-database
```

#### 3. Change path to source file
The project contains a default source file with over 500 questions. You can add these questions to the database in the application.

However, you must first specify the correct path to the file. To do this, you must change the following line in appsettings.json:

```json
  "SourceFilePath": {
    "Default": "C:\\Users\\user\\source\\repos\\WhoWantsToBeAMillionaireGame\\WhoWantsToBeAMillionaireGame\\source.json"
  },
```
>**Note**
> You can specify the path to your question source, but make sure the JSON file structure matches the one below.
>
>```json
>[{
>  "question": "Someone question?",
>  "A": "first answer",
>  "B": "second answer",
>  "C": "third answer",
>  "D": "fouth answer",
>  "answer": "A"
>}, {
>  "question": "Another question?",
>  "A": "first answer",
>  "B": "second answer",
>  "C": "third answer",
>  "D": "fouth answer",
>  "answer": "C"
>}]
>```


>**Important**
>The question with the same question text or with two or more same answers don't pass validation and don't put to database.

## Description of the project architecture.
### Summary
The application is based on ASP.NET Core MVC and Microsoft SQL Server. Entity Framework Core is used to work with the database. The interaction between the application and the database is done using the Generic Repository and Unit of Work. JSON file parsing is done with NewtonSoft JSON.Net library.

Data for each game is generated at the start of the game and stored in the database.

The application writes logs to a new file for each run. Logging is based on the Serilog library.

The client-server interaction is based on sessions and JS scripts and endpoints interaction in the controller.

Key functions of the server part:
- generic repository
- unit of work
- custom JSON converter
- patch model and patching
- custom session class and session extension;

### Database model
Database contains four entities: 
- Question: keeps question text and state (enable/disable) 
- Answer: keeps text and the flag IsCorrect and Question Id
- Game: keeps only Id. The game entity is added for the possibility of later expansion of functionality such as adding cues (like 50:50). 
- GameQuestion: keeps QuestionId, GameId and the flag IsSuccessful.

### Composition of solution
The solution contains the main project and several libraries:

- **WhoWantsToBeAMillionaireGame:** main project
- **WhoWantsToBeAMillionaireGame.Business:** contains the basic business logic of the application not directly related to MVC (services implementations and custom JSON converter)
- **WhoWantsToBeAMillionaireGame.Core:** contains entities that do not depend on the implementation of other parts of the application (interfaces of services, data transfer objects, patch model)
- **WhoWantsToBeAMillionaireGame.Data.Abstractions:** contains interfaces for database logic utils
- **WhoWantsToBeAMillionaireGame.Data.Repositories:** contains implementation of Data.Abstractions
- **WhoWantsToBeAMillionaireGame.DataBase:** contains entities and DBContext class

### Controllers
The WhoWantsToBeAMillionaireGame contains three controllers:
- **HomeController:** logic for the main page 
- **QuestionController:** provides logic for creating, deleting, changing state (enable/disable), and loading from the source file of questions
- **GameController:** making the game page and providing some endpoints (API-like) for clint-side requests

## Key features:
ASP.Net Core MVC, Entity Framework Core, Microsoft SQL Server, C#, JavaScript, Serilog, Automapper, Newtonsoft Json.Net, Dependepcy Injection, Generic Repository, Unit of Work, Bootstrap



