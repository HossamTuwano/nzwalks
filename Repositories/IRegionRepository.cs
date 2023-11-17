using NZWalks.Models.Domain;

namespace NZWalks.Repositories;


public interface IRegionRepository
{
    Task<List<Region>> GetAllAsync();
}
