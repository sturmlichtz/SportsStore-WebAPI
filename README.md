# SportsStore WebApp


#### Solution SSWebAPISolution

- dotnet new sln --name SSWebAPISolution --output SSWebAPISolution
- cd SSWebAPISolution

#### Project SSWebAPIApp (ASPNetCore WebApp)

- dotnet new web --name SSWebAPIApp
- dotnet sln add SSWebAPIApp

#### Packages for the SSWebAPIApp

- dotnet add SSWebAPIApp package Swashbuckle.ASPNetCore --version 6.1.4
- dotnet add SSWebAPIApp package Microsoft.EntityFrameworkCore --version 5.0.7
- dotnet add SSWebAPIApp package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.7
- dotnet add SSWebAPIApp package Microsoft.EntityFrameworkCore.Design --version 5.0.7

- How to list packages installed in the project
  - dotnet list <projectname> package

#### Dotnet tool for the project SSWebAPIApp

- dotnet tool list --global (will give the list of global tools installed)
- dotnet ef tool
  - dotnet tool install/uninstall --global dotnet-ef

#### dotnet ef commands

- dotnet ef migrations add InitialDb --output-dir Models/Migrations/SSMigrations --context <context class name> --project <project name>
- dotnet ef migrations remove --context <context class name>
- dotnet ef database update (will create the database and the table/s, reading from the Models/Migrations/SSMigrations)
- dotnet ef database drop (will drop the database)

#### Git Branches

- 01Start
- 02ModelEntities
- 03DbContext
- 04DotNetEFTool
- 05SportsStoreSeedData
- 06CreateRepository
- 07API
- 08Swagger