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
> A DbContext class in Entity Framework (EF) or EF Core is a central class that manages the database connection and serves as a bridge between your domain models and the database. It provides methods for querying and saving data, tracking changes, and managing transactions.

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

     
     services.AddScoped<IMyService, MyService>();
     

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

----------------------------------------------------------------------------

### 11. Asynchronous Programming
> Asynchronous programming is a programming paradigm that allows a program to perform tasks concurrently without blocking the main thread. It enables non-blocking operations, improving responsiveness and performance, especially in I/O-bound applications.

> In C#, asynchronous programming is primarily achieved using the `async` and `await` keywords, allowing methods to run in the background while the main thread continues executing other tasks.

> This is useful for tasks like file I/O, network requests, and database operations, where waiting for a response can lead to inefficiencies.

- Use `async` and `await` keywords in your methods to make them asynchronous.

- Change the return type of your methods to `Task<IActionResult>` instead of `IActionResult`.
- Use `await` when calling asynchronous methods, such as `ToListAsync()` or `FirstOrDefaultAsync()`, to ensure the method waits for the result without blocking the main thread.
- Import the `Microsoft.EntityFrameworkCore` namespace to access the asynchronous methods provided by Entity Framework Core.
- Example:
    ```
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var regions = await dbContext.Regions.ToListAsync();
        return Ok(regions);
    }
    ```




### 12. Repository Pattern

> The Repository Pattern is a design pattern used in software development, especially in .NET applications, to abstract the data access logic and centralize data operations.

## ✅ Definition:
> The Repository Pattern separates the business logic from the data access logic by abstracting the interaction with the data source (like a database) into a separate repository class.


<img src="./Assets/Repository Pattern.jpg" alt="My Image" />

### Benifits
- Decoupling business logic from data access logic.
- Promotes cleaner code and better organization.
- Easier to manage and maintain data access logic.
- Performs CRUD operations without exposing the underlying data source details.
- Multiple Data Sources: Can work with different data sources (e.g., SQL, NoSQL) without changing the business logic.

- Design pattern to separate the data access layer from the application.
- Provides interface without exposing implementation. 
- Helps create abstraction.
- Separation of concerns (clean architecture)
- Easier to test (e.g., with mock repositories)
- Helps manage data access logic in one place
- Simplifies unit of work and transaction management

## 🔹Structure Overview:
### 1. Model (Entity) – Represents the data.

### 2. IRepository (Interface) – Defines methods like Add, Get, Update, Delete.

### 3. Repository (Implementation) – Implements IRepository and interacts with the database (e.g., using EF Core).

### 4. Service Layer or Controller – Uses the repository to perform operations.

## 🔸 Example in C# (.NET Core)

### 1. Model :

```
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### 2. IRepository Interface :
```
public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
```

### 3. Repository Implementation :
```
public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public IEnumerable<T> GetAll() => _dbSet.ToList();

    public T GetById(int id) => _dbSet.Find(id);

    public void Add(T entity) => _dbSet.Add(entity);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);
}
```

### 4. Usage in Service or Controller :
```
public class ProductService
{
    private readonly IRepository<Product> _productRepo;

    public ProductService(IRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    public void CreateProduct(Product product)
    {
        _productRepo.Add(product);
    }

    public IEnumerable<Product> GetAllProducts()
    {
        return _productRepo.GetAll();
    }
}
```

Steps yo Do it in the Project:
1. Create a folder named `Repositories`.
1. Create an interface `IRegionRepository.cs` inside the `Repositories` folder.
1. Define the methods/defination you want to implement in the repository, such as `GetAllAsync`, `GetByIdAsync`, `AddAsync`, `UpdateAsync`, and `DeleteAsync`.
1. Create a class `SQLRegionRepository.cs` that implements/Inherit the `IRegionRepository` interface.

    > use `ctrl` + `.` to import the interface.
1. Inject the `NZWalksDbContext` into the `RegionRepository` constructor.
    ```
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync(); // Fetches all regions from the database asynchronously.
        }

        // Additional methods for CRUD operations can be added here, such as GetByIdAsync, AddAsync, UpdateAsync, DeleteAsync, etc.

    }
    ```
1. Implement the methods in the `SQLRegionRepository` class using the `dbContext` to perform CRUD operations.
    ```
    public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync(); // Fetches all regions from the database asynchronously.
        }
    ```
1. In the `Program.cs` file, register the repository with the dependency injection container using `services.AddScoped<IRegionRepository, SQLRegionRepository>();`.
In the `RegionsController`, inject the `IRegionRepository` instead of `NZWalksDbContext` and use it to perform operations.
   ```
    builder.Services.AddScoped<IRegionRepository, SQLRegionRepository>();
    ```
1. In the `RegionsController`, inject the `IRegionRepository` instead of `NZWalksDbContext` and use it to perform operations.
    ```
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        public RegionsController(IRegionRepository regionRepository)
        {
            this.regionRepository = regionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var regions = await regionRepository.GetAllAsync();
            return Ok(regions);
        }
    }
    ```
1. Now, you can use the `IRegionRepository` in your controller to perform CRUD operations on regions without directly accessing the `NZWalksDbContext`. This promotes separation of concerns and makes your code cleaner and more maintainable.
1. You can now implement other methods like `GetByIdAsync`, `AddAsync`, `UpdateAsync`, and `DeleteAsync` in the `SQLRegionRepository` class, following the same pattern as shown above.
1. You can also create similar repositories for other domain models like `Difficulty` and `Walk` by following the same steps.

EXTRA : 
- Now our `Controller` calls the `Repository` and `Repository` calls the `DbContext`. This is called the **`Repository Pattern`**. 
- Now you can switch the `dbContext` to any other database like `MongoDB`, `PostgreSQL`, etc. without changing the `Controller` code. This is called **`Loose Coupling`**.
- You can also create a `MockRepository` for testing purposes without changing the `Controller` code. This is called **`Dependency Injection`**.

Now, Let's say business decides no to use `SQL Server` but to use `InMemoryDataBase`. 

To demonstate create a another concreate implemetation of the `IRegionRepository` interface called `InMemoryRegionRepository.cs`.

- Create a new class `InMemoryRegionRepository.cs` inside the `Repositories` folder.
- Implement the `IRegionRepository` interface in the `InMemoryRegionRepository` class.
    > use `ctrl` + `.` to import the interface and use the option `Implement all Method explicitely`.  
    ```
     return Task.FromResult(new List<Region>()
            {
                new Region()
                {
                    Id = Guid.NewGuid(),
                    Code = "NI",
                    Name = "North Island",
                    RegionImageUrl = "https://example.com/north-island.jpg"
                },
                new Region()
                {
                    Id = Guid.NewGuid(),
                    Code = "SI",
                    Name = "South Island",
                    RegionImageUrl = "https://example.com/south-island.jpg"
                }
            });
    ```
- Replace the IRegionRepository with the InMemoryRegionRepository in the Program.cs file.
    ```
    builder.Services.AddScoped<IRegionRepository, InMemoryRegionRepository>();
    ```
- Done and now roll it back as it for a Demonstration purpose only.
### 12. AutoMapper