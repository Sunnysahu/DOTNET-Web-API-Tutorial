using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Data
{
    public class NZWalksDbContext : DbContext
    {

        public NZWalksDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

           
            
        }
        // These 3 Representing Tables in the SQL Server
        // Now we need columns, so we need to create properties for each of the tables
        // now we need to create properties for each of the tables
        // DbSet is a class that represents a collection of entities in the database.
        public DbSet<Difficulty> Difficulties { get; set; }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }
    }
}
