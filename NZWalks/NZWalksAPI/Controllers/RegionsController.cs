using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Controllers
{   
    // This attribute defines the route for the controller. The [controller] token will be replaced with the name of the controller, in this case, "Regions".
    // http://localhost:1234/api/regions
    [Route("api/[controller]")] 
    [ApiController] // This attribute indicates that this class is a controller for an API, It automatically Validates the model state and return backs 400 to the caller, if the model state is invalid.
    public class RegionsController : ControllerBase
    {

        [HttpGet] // This attribute indicates that this method will respond to HTTP GET requests.
        public IActionResult GetAll()
        {
            var region = new List<Region>
            {
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Jamshedpur",
                    Code = "JAM",
                    RegionImageUrl = "https://s7ap1.scene7.com/is/image/incredibleindia/jubilee%20Park-jamshedpur-jharkhand-hero?qlt=82&ts=1726724218264"
                },
                new Region
                {
                    Id = Guid.NewGuid(),
                    Name = "Shahdol",
                    Code = "SHD",
                    RegionImageUrl = "https://captureatrip-cms-storage.s3.ap-south-1.amazonaws.com/Jubilee_Park_2d81800cdc.webp"
                }
            };
            return Ok(region);
        }
    }
}
