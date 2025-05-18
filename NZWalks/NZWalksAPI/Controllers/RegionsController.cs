using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetAll() // This method is an asynchronous action that returns a list of all regions.
        {
            // Get Data from the database -- Domain Models.

            var regionsDomain = await dbContext.Regions.ToListAsync(); // Fetches all regions from the database. 

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

        [HttpPost] // This attribute indicates that this method will respond to HTTP POST requests.
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map/Convert AddRegionRequestDto to Region Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // Use Domain Model to create a new region

            dbContext.Regions.Add(regionDomainModel); // Add the new region to the database context.

            dbContext.SaveChanges(); // Save the changes to the database.

            // Map/Convert Region Domain Model to Region DTO

            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }

        // http://localhost:portnmber/api/region/{guid}
        [HttpPut] // This attribute indicates that this method will respond to HTTP PUT requests.
        [Route("{id:Guid}")] // This attribute defines the route for this specific action method. The {id} token will be replaced with the actual ID value passed in the URL.
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto )
        {
            // Fetch the region from the database
            var regionDomainModel =  dbContext.Regions.FirstOrDefault(x => x.Id == id); // LINQ Code --> Fetches a single region by its ID from the database.

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Map/Convert UpdateRegionRequestDto to Region Domain Model

            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

            dbContext.SaveChanges(); // Save the changes to the database.

            // Map/Convert Region Domain Model to Region DTO

            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);
        }

        [HttpDelete] // This attribute indicates that this method will respond to HTTP DELETE requests.
        [Route("{id:guid}")] // This attribute defines the route for this specific action method. The {id} token will be replaced with the actual ID value passed in the URL.
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel =  dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            dbContext.Regions.Remove(regionDomainModel); // Remove the region from the database context.
            dbContext.SaveChanges();

            // Map/Convert Region Domain Model to Region DTO

            var regionDto = new RegionDto()
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto); // Return the deleted region as a response.

        }
    }
}
