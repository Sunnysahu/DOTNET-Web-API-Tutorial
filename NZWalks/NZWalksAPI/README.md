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
6. Add the Connection String