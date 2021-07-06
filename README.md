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

- **Packages for Identity Package**
  - dotnet add SSWebAPIApp package Microsoft.Extensions.Identity.Core --version 5.0.7 (will contain the Identity Features)
  - dotnet add SSWebAPIApp package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 5.0.7 (will contain features to store data in database using EntityFrameworkCore)

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
- **Migrations for Identity**
  - dotnet ef migrations add InitialIdentityDb --context IdentityDbContext --output-dir Models/Migrations/SSIdentityMigrations  

#### Git Branches

- 01Start
- 02ModelEntities
- 03DbContext
- 04DotNetEFTool
- 05SportsStoreSeedData
- 06CreateRepository
- 07API
- 08Swagger
- 09CORSAndSPA
- 10ConfigureSecurity
- 11AccountController
- 12JQuery