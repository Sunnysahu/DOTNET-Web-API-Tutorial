# Hello Guys, This is a Web API Project

<img src="./Assets/NZWalks DDD.jpg" alt="My Image" />

# Create Domain Models Based on Above Image

- ### 1. Create Model Folder --> Domain Folder --> Difficulty.cs, Walk.cs & Region.cs class 
- ### 2. Write the Your Code and then build a Releationship b/w these Domans Model

### 3. Add/ Install Entity Framework Core

- Right Click on the Dependencies --> Manage NuGet Packages --> Search for 
``` 
Microsoft.EntityFrameworkCore.SqlServer 
``` 
and 
```
Microsoft.EntityFrameworkCore.Tools 
```
### 4. Create a Db Context Class
> The `DbContext` class in Entity Framework and EF Core connects your .NET application to the database, managing connections, mapping entities to tables via `DbSet<T>`, and enabling CRUD operations. It supports LINQ queries, tracks changes, and persists data with `SaveChanges`, simplifying database interactions and boosting productivity.

>It also Acts as a Bridge b/w Domain Model and Database.

> Controller <--> DbContext <--> Database	


- Create a folder name `Data`. Add a new Class `NZWalksDbContext.cs`.
- Inherit the `DbContext` class. Use ctrl + . for importing the entity framework core.
- Create a Constructor, using `ctor`, Pass the DbContextOptions and Some Name.

<hr/>

### 5. Add the DbSet Properties
> A DbSet<T> in Entity Framework (EF) or EF Core is a property within a DbContext class that represents a collection of entities of type T, corresponding to a database table. It enables querying, adding, updating, and deleting records using LINQ, with changes tracked by the DbContext.

Add the `DbSet` properties for each of the domain models. Add Code Similar to This : 

```
 public class NZWalksDbContext: DbContext 
 {

   public NZWalksDbContext(DbContextOptions dbContextOptions): base(dbContextOptions) 
   {

   }

   public DbSet <Difficulty> Difficulties {get; set;}

   public DbSet <Region> Regions {get; set;}

   public DbSet <Walk> Walks {get; set;}
 }
```
 
<hr/>

### 6. Add the Connection String
  - Open the `appsettings.json` file and add a connection string below the `AllowedHost`.

    ```
    {
      "ConnectionStrings": {
        "<Conn.String__Name>": "Server=YOUR__SERVER_NAME;Database=YOUR_DB_NAME;Trusted_Connection=True;TrustServerCertificate=True;"
      }
    }
    ```
 
<hr/>

### 7. Add the Connection String to the DbContext (Dependency Injection)
---------------------------------------------------------------------------------
> Dependency Injection is a design pattern used to achieve Inversion of Control (IoC) between classes and their dependencies.

> Instead of a class creating its dependencies, they are provided (injected) from the outside.

> DI works on this fundamental that instead of instantiating objects within a class those objects are passed in as parameters to the class like passing it to the constructor or the method instead.

> 🧠 Real-Life Analogy: A Car doesn’t build its own engine. The engine is installed from outside (dependency). This way, you can install any type of engine without changing the car's logic.
---------------------------------------------------------------------------------
| Benefit                 | Description                                                             |
|-------------------------|-------------------------------------------------------------------------|
| ✅ Loose coupling        | Class depends on abstractions (interfaces), not concrete classes.       |
| ✅ Easier testing        | You can inject mock dependencies during testing.                        |
| ✅ Better maintainability| Changes in one class don't ripple across the system.                    |

- Example 
```
public class MyConrtoller: ConrollerBase {

  private readonly MyService _service;

  public MyController() 
  {
    _service = new MyService();
  }

  public IActionResult Index() 
  {
    var data = _service.GetData();

    return Ok(data);
  }
}      
```

 How to do it with Dependency Injection --> Program.cs file and add 

     ```
     services.AddScoped<IMyService, MyService>();
     ```

- Open the `Program.cs` file and add the following code to the `builder.Services` section:
    ```csharp

    builder.Services.AddDbContext<NZWalksDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringName")));

    ```
 By this we are telling the application to use the `NZWalksDbContext` class and use the connection string from the `appsettings.json` file.

 <hr/>

### 8. Add the Migrations

- Open the NuGet Package Manager Console and run the following commands:
    ```
    Add-Migration "<Name_of_Migration>"
    ```
- You Will have the new `Migration Folder` Created and Some Files with out Specified Name in it.
- Now run the Migration, as we don't have anyting and after running A `Database` will be created in the SQL Server with the name you provided in the connection string.
- 
    ```
    Update-Database
    ```
- You can check the database in the SQL Server Management Studio.
<hr/>

### 9. CRUD Operation Regions Table
- Create a new folder `Controllers` and add a new class `RegionsController.cs`.
- Create a constructor and inject the `NZWalksDbContext` class. This is Called Constructor Injection (CI);
- To get result from DB, use Db Context to insert Data and Retreive Data. We can use Db Context Here as we did a Dependency Injection in `Program.cs` file 
- Create a Private variable (named `dbContext`)and from constructor assign the value to it.
- Now use that `dbConext` to access Data from MySQL Server.
> Just to check, manually insert the data in the `Regions` table using SQL Server Management Studio

```
Insert into regions ([Id], [Code], [Name], [RegionImageUrl]) values ('0622bc15-72f1-48be-b44d-bf25fb874eee', 'JSR', 'Jamshedpur', 'https://captureatrip-cms-storage.s3.ap-south-1.amazonaws.com/Jubilee_Park_2d81800cdc.webp');
```

- Now, Implement Get Region by Id Action Method

> Tip : Use [FromRoute] when:

    Parameter name ≠ route token

    You want code to be clear and explicit

    There are multiple sources (e.g., [FromBody], [FromQuery], [FromHeader], [FromX] etc.)

 if you name the parameter differently:
 ```csharp
 [HttpGet("{regionId}")]
 public IActionResult GetById([FromRoute(Name = "regionId")] Guid id)
 ```
 You must use [FromRoute] here, or the binding will fail because regionId ≠ id.

 You can also use `LINQ` Query to do this by using this Code : 
 ```
  var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);
 ```
 <hr/>

### DTOs vs Domain Model

<img src="./Assets/DTO vs DM.jpg" alt="My Image" />

> DM --> A domain model represents the actual data and behavior in your application — the "real" structure of your business logic. It contains all fields and methods that define how your system works.

🔧 Real-Life Example:
Imagine you’re building an Employee Management System.
A domain model might look like this:

```
public class Employee
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public decimal Salary { get; set; }
    public string Role { get; set; }

    public bool IsManager()
    {
        return Role == "Manager";
    }
}
```
This model contains everything about the employee — even things like salary, which may be sensitive.

> DTO -->  A DTO is a lightweight object that is used to transfer data between layers (e.g., from your backend to frontend or API). It contains only the necessary information, and usually no logic.
<hr/>

💡 Real-Life Example:

Let’s say you’re showing a public employee profile on a website. You don’t want to show the salary or internal ID.

A DTO might look like this:
```
public class EmployeeDto
{
    public string FullName { get; set; }
    public string Role { get; set; }
}
```
This way, you’re not exposing sensitive data like Salary or Email to the outside world. It’s safer and cleaner.

✅ Why use both?
Use Domain Model inside your application (business logic, DB interaction).

Use DTOs to send/receive data through APIs or UI — keeping it simple, secure, and minimal.
🔄 Mapping between Domain and DTO:
```
var dto = new EmployeeDto
{
    FullName = employee.FullName,
    Role = employee.Role
};
```
> Till now we're getting all the region back and we're sending the Domain Mode back to client (swagger). This is coupling of the Domain Model to the API View Layer. 

> We've to convert Domain Model to DTOs and Expose DTOs to the outside model instead.

- Get Data from the database -- Domain Models.
- Map Domain Models to DTOs
- Return DTOs (Never Return back Domain Model to the Client)

### 10. DTO Setup

- Create a `DTO` Folder and Inside that create a `RegionDto.cs` file.
- Create a class `RegionDto` and add the properties you want to expose to the client.
