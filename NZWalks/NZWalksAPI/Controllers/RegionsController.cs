using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.DTO;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Controllers
{
    // This attribute defines the route for the controller. The [controller] token will be replaced with the name of the controller, in this case, "Regions".
    // http://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController] // This attribute indicates that this class is a controller for an API, It automatically Validates the model state and return backs 400 to the caller, if the model state is invalid.
    public class RegionsController : ControllerBase
    {

        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet] // This attribute indicates that this method will respond to HTTP GET requests.
        public IActionResult GetAll()
        {
            // Get Data from the database -- Domain Models.

            var regionsDomain = dbContext.Regions.ToList(); // Fetches all regions from the database. 

            // Map Domain Models to DTOs
            var regionsDto = new List<RegionDto>(); // Create a new list to hold the mapped DTOs.
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }

            // Return DTOs (Never Return back Domain Model to the Client)
            return Ok(regionsDto);
        }
        
        // GET Signle Region or GET Region by ID
        // http://localhost:1234/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")] // This attribute defines the route for this specific action method. The {id} token will be replaced with the actual ID value passed in the URL.
        public IActionResult GetById([FromRoute] Guid id)
        {
            // var region = dbContext.Regions.Find(id); // Fetches a single region by its ID from the database.

            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id); // LINQ Code --> Fetches a single region by its ID from the database.

            if (regionDomain == null)
            {
                return NotFound(); 
            }

            // Map/ Convert Region Domain Mode to Region DTO

            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            
            return Ok(regionDto); 
        }
    }
}
