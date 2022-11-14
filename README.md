# WhoWantsToBeAMillionaireGame
The project implements a simplified version of Who Wants to Be a Millionaire

## How to configurate
### Configurate database
#### 1. Change connection string
Firstly, you should change connection string in appsettings.json

```json
"ConnectionStrings": {
    "Default": "Server=myServer;Database=myDataBase;User Id=myUsername;Password=myPassword;TrustServerCertificate=True"
  },
```

>**Note**
> The project is designed to use Microsoft SQL.Server version 15 or higher. For versions below 15 stable operations is not guaranteed. The project uses the GUID type as the primary and therefore using other database providers requires additional changes and adjustments.

#### 2. Initiate database

In Visual Studio open Packege Manager Console (VIew -> Other Windows -> Packege Manager Console). Choose WhoWantsToBeAMillonaireGame.DataBase in Default project.

// todo: add screenshot here

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

## Key features:
ASP.Net Core MVC, Entity Framework Core, Microsoft SQL Server C#, Serilog, Automapper, Newtonsoft Json.Net, Dependepcy Injection, Generic Repository, Unit of Work, Bootstrap



