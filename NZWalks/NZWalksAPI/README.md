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

### 5. Add the DbSet Properties
> A DbSet<T> in Entity Framework (EF) or EF Core is a property within a DbContext class that represents a collection of entities of type T, corresponding to a database table. It enables querying, adding, updating, and deleting records using LINQ, with changes tracked by the DbContext.

- Add the `DbSet` properties for each of the domain models. Add Code Similar to This : 
    ```
    public class NZWalksDbContext : DbContext
        {

            public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
            {

           
            
            }

            public DbSet<Difficulty> Difficulties { get; set; }

            public DbSet<Region> Regions { get; set; }

            public DbSet<Walk> Walks { get; set; }
        }
    ```
### 6. Add the Connection String
- Open the `appsettings.json` file and add a connection string below the `AllowedHost`.
    ```
    {
      "ConnectionStrings": {
        "<Conn.String__Name>": "Server=YOUR__SERVER_NAME;Database=YOUR_DB_NAME;Trusted_Connection=True;TrustServerCertificate=True;"
      }
    }
    ```

### 7. Add the Connection String to the DbContext (Dependency Injection)
---------------------------------------------------------------------------------
> Dependency Injection is a design pattern used to achieve Inversion of Control (IoC) between classes and their dependencies.

> Instead of a class creating its dependencies, they are provided (injected) from the outside.

> DI works on this fundamental that instead of instantiating objects within a class those objects are passed in as parameters to the class like passing it to the constructor or the method instead.

> 🧠 Real-Life Analogy: A Car doesn’t build its own engine. The engine is installed from outside (dependency). This way, you can install any type of engine without changing the car's logic.
---------------------------------------------------------------------------------

| Benefit                   | Description |
-----------------------------------------------------------------------------------------------|
| ✅ Loose coupling         | Class depends on abstractions (interfaces), not concrete classes.|
| ✅ Easier testing         | You can inject mock dependencies during testing.|
| ✅ Better maintainability | Changes in one class don't ripple across the system. |

- Example 
    ```
        public class MyConrtoller : ConrollerBase
        {
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

 How to do it with Dependency Injection --> o Program.cs file and add 

     services.AddScoped<IMyService, MyService>();


- Open the `Program.cs` file and add the following code to the `builder.Services` section:
    ```csharp

    builder.Services.AddDbContext<NZWalksDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringName")));

    ```
 By this we are telling the application to use the `NZWalksDbContext` class and use the connection string from the `appsettings.json` file.

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