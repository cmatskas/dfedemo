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
<PropertyGroup>        
    <TargetFramework>netcoreapp2.0</TargetFramework>
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

You also need to ensure that the new pages are accessible from the Navigation Bar. Find the `_Layout.cshtml` and add the missing links to your new application sections.

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

**PartialViews** have limited functionality whereas **Web Components** are a lot more powerful and flexible. On the other hand,

> View Components (VCs) are similar to Partial Views, but they are much more powerful. VCs include the same separation-of-concerns and testability benefits found between a controller and view. You can think of a VC as a mini-controller—it’s responsible for rendering a chunk rather than a whole response. 

To make search more efficient we need to think how to query against our existing data store. Although we could use several queries and combine them together, we want to minimise round trips to the database and use as little code as possible. Alternatively, we could use a 3rd party search component to remove the need for extra coding. However, for this project we assume that all the features will be provided by the application natively.

In the project, right-click and add a new Controller -> **SearchController**. Add a constructor to allow you to inject the appropriate DBContext. Then create an **Index()** method that will be used to load the Search page. The **Index()** should pass a model for model-binding the search criteria.

Create a new class `SearchCriteria.cs` with the fields necessary to run your search. At this point you also want to think how to pass the `LA` data during page load. The LA data should come from the database and passed to the view so that the user can select the appropriate value from the drop down. 

Add the following code in the `SearchCriteria` class:

```
public class SearchCriteria
{
    public string Name { get; set; }
    public List<Ladata> LaData { get; set; }
    public int LaValue { get; set; }
    public bool CaseStatus { get; set; }
    public bool Absence { get; set; }
    public bool Bullying { get; set; }
    public bool Other { get; set; }
}
```

As a next step, we want to implement the Index method for the `HttpGet` operation. The Index action should look like this:

```
[HttpGet]
public async Task<IActionResult> Index()
{
    var searchCriteria = new SearchCriteria()
    {
        LaData = await dbContext.Ladata.ToListAsync()
    };

    return View(searchCriteria);
}
```

You'll notice that this controller action makes a call to the database to pull and populate the `LaData` field. This is where, usually, we make use of dependency injected services to interact with the database. But this is beyond the 100-level or the simplicity of this specific PoC. 

The above action returns a View so if you haven't created one yet, you need to do it now. Remember that ASP.NET Core uses *convention over configuration* and it makes certain assumptions about the location of your files. For the view to be accessible we need to place the code in the right location. Create a new directory structure in your ASP.NET Core project: **Views -> Search**. Right-click on the **Search** folder and add a new **View**. The View should be strong-typed against the `SearchCriteria` class. Scaffolding should take care of creating the right layout, however, you need to change the LaValue to use a drop-down. We can leverage the new **tag-helpers** built-into the ASP.NET Core framework and Razor Views to create the drop-down control:

```
<div class="form-group">
    <label asp-for="LaValue" class="control-label"></label>
    <select asp-for="LaValue" asp-items="@(new SelectList(Model.LaData,"Id","Name"))" class="form-control"></select>
    <span asp-validation-for="LaValue" class="text-danger"></span>
</div>
```

Next, we need to implement the action in the Search controller that will accept the `HttpPost` from our advanced search form. Add a new `Index()` action with a `SearchCriteria` object as a parameter and implement the actual search operation based on the user-provided values. 

```
public async Task<IActionResult> Index(SearchCriteria searchCriteria)
{
    if (ModelState.IsValid)
    {
        var results = await GetUserCaseData(searchCriteria);
        return View("Results", results);
    }

    return View(searchCriteria);
}
```

One way to implment the actual **Search** functionality is to use a EF Core with Linq and rely on the `IQueryable` to build up the query. This way we can add the paramater values as they were passed by the user in the `SearchCriteria` object:

```
 private async Task<List<UserCase>> GetUserCaseData(SearchCriteria searchCriteria)
{
    var query = dbContext.UserCase.AsQueryable();

    if (!string.IsNullOrEmpty(searchCriteria.Name))
    {
        query = query.Where(s => s.Firstname == searchCriteria.Name);
        query = query.Where(s => s.Surname == searchCriteria.Name);
    }
    
    query = query.Where(s => s.Laid == searchCriteria.LaValue);
    query = query.Where(s => s.Absence == searchCriteria.Absence);
    query = query.Where(s => s.Bullying == searchCriteria.Bullying);
    query = query.Where(s => s.Other == searchCriteria.Other);

    return await query.ToListAsync();
}
```

Finally, we need to create the **View** to display the search results.

In the **Views** folder, create the following folder structure: **View -> Search -> Results**. Right click on the **Search** folder and add a new View. Use scaffolding to speed up the process. In the new dialog box, choose the following values:

- ViewName: Results
- Template: List
- Model class: UserCase
- DataContext class: DataDbContext

Edit the generated view to give it the look and feel you wish. Also make sure to remove the CRUD operations you don't need. 
