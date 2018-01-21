## 1. Authentication

Start by creating a new project in Visual Studio. Although you can achieve the exact same from the command line, VS2017 makes life easier and more productive

In VS2017 go to:

**File -> New Project -> ASP.NET Core**

On the right hand side, choose the authentication appropriate to your project

**Authentication -> Individual User Accounts**

You need to think about security. If you open the `application.json` you'll notice that the connection string to the database is in clear text. We need to secure this in the local development environment and avoid committing any sensitive credentials in source control. 

ASP.NET Core and VS2017 come with a secrets manager. So, before running the application, migrate the connections strings to the secrets file and away from the source code.

First, you need to check the `csproj` file to ensure that the `UserSecretsId` is setup. If not add this to the `csproj` file:

```
<PropertyGroup>        <TargetFramework>netcoreapp2.0</TargetFramework>
    <UserSecretsId>User-Secret-ID</UserSecretsId>
</PropertyGroup>
```

Next, open the CLI, navigate to the ASP.NET project level and type the following command:

```
dotnet user-secrets set  DefaultConnection  <yourDBConnectionString>
```

The command accepts the following requirements:

`dotnet user-secrets set ParameterName ParameterValue`

>Important! If you're using the CLI to set up variables, the tool will try to escape '\\' turning them into '\\\\'. This will break the connection string which expects `(localdb)\\`. We can overcome this by setting it to a single quote and let the CLI escape it to the correct format

Next, in the `Startup.cs` constructor we need to update the code to look like this:

```
public Startup(IConfiguration configuration, IHostingEnvironment env)
{
    Configuration = configuration;

    var builder = new ConfigurationBuilder();

    if (env.IsDevelopment())
    {
        builder.AddUserSecrets<Startup>();
    }

    Configuration = builder.Build();
}
```

Save and run the code.

## 2. DataModel

The DataModel and various application data should not be stored with our user identity database for security reasons. Segregating the data helps pretect against malicious attacks. 

For the purpose of this exercise, we'll assume that the application data lives in a separate database. ASP.NET Core has great support for EF Core, which is a modern ORM. 

Instead of manually adding the **POCO** classes to map to the existing database, we can use the EF Core scaffolding tools to create the necessary models and DBContext.

The `Microsoft.ASPNETCore.All` NuGet metapackage includes all the necessary packages and namespaces to allow us to work with EF Core and the scaffolding. If they are not avaialble (highly unlikely), you need the following NuGet packages using the CLI. Alternatively, you can use the VS package manager:

```
dotnet add package Microsoft.EntityFrameworkCore 
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools.DotNet-v 2.0.0
```
The packages are not enough though. EF Core has a .NET tool that makes the EF commands available to the `dotnet` CLI. The tool comes in teh form of a NuGet package so you'll need to open the `csproj` file and add the following code:

```
<ItemGroup> 
   <DotNetCliToolReference Include="Microsoft.EntityFrameworkCore.Tools.DotNet" Version="2.0.0" /> 
</ItemGroup>
```

To scaffold our database code, i.e. create the necessary DBContext and POCOs, we need to use the command line. Make sure you navigate to the ASP.Net WEB APP directory and run the following command:

```
dotnet ef dbcontext scaffold <connectionString> Microsoft.EntityFrameworkCore. SqlServer -o Models -f -c DemoDbContext 
```
An working example is attached below:

```
dotnet ef dbcontext scaffold "Server=(localdb)\mssqllocaldb;Database=aspnet-DfeDemo-UserData;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -f -c DataDbContext
```

With the new DBContext in place, we need to add the new connection string to our secrets file. Remember that this is a different database and we want to keep them separate. 

Follow the steps from exercise 1

At this point, you can choose whether you want to move the new POCO classes to a **NETSTandard** project or not. This is an architectural decision. It may be premature at this stage of the project but we want to keep best practices in mind. 

To help with the MVC scaffolding later, we need to ensure that all the newly generated classes have a default constructor. Add the constructors as necessary. 

In `Startup.cs`, add a new code in the `ConfigureServices()` method for the new DBContext we just created

```
services.AddDbContext<DataDbContext>(options => options.UseSqlServer(Configuration["DataConnection"]));
```

## 3. Adding CRUD Views and Controllers

In Visual Studio, right-click on the **Controllers** folder and choose **"Add new Scaffolded item…"**. 
Next, choose the **"MVC Controller with views using Entity Framework"**.

On the next dialog, choose the appropriate model class and DBContext in order to generate the appropriate views and controller actions.

For each controller you generate, you may want to run the application and test the various operations/actions. You may also want to edit the Views to have the look and feel you wish (as per your requirements).

## 4. Securing access to your data
To restrict access to parts of your application, you need to use the built-in authantication and authorisation framework. Navigate to the controller you wish to secure and add the [Authorize] annotation. For example, to protect all actions in the `LadataController`, the code should look like this:

```
[Authorize]
public class LadataController: Controller
```

Test the controller to ensure that it blocks access to unauthenticated users.

## 5. Adding model validation

Navigate to the LaData **Create** page and attempt to create a new `ladata` item without populating the **Name** value in the form! whoops!
ASP.NET can enforce form validation both on the client and the server side. On the client side, we rely on **jQuery** while on the server we rely on **Model-Binding**. We need to add model validation both on the client and the server because JavaScript can be circumvented. 

Open the `lademo.cs` class and add the `[Required]` data annotation. Add the necessary namespaces to ensure the code compiles. The annotation should be added to all the necessary properties. Run the application again to test that validation works.

## 6. Adding a Search feature
Unlike in the previous modules that we relied on the ASP.NET Core framework to do the heavy lifting with the scaffolding, The search is a bespoke piece of work. For this bit, we need to create a controller,  ViewModels and the EF Queries to pull the data. If we want to make search available throughout the site, we could look into implementing one of the following:

- PartialView
- Web Component

**PartialViews** have limited functionality whereas **Web Components** are a lot more powerful and flexible. 