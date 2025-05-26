using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class InmemoryRegionRepository : IRegionRepository
    {
        public Task<List<Region>> GetAllAsync()
        {
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
        }
    }
}
