using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Models.Domain;

namespace NZWalks.Repositories;

public class SQLWalkRepository : IWalkRepository
{
    private readonly NZWalksDbContext dbContext;
    public SQLWalkRepository(NZWalksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }


    public async Task<Walk> CreateAsync(Walk walk)
    {
        await dbContext.Walks.AddAsync(walk);
        await dbContext.SaveChangesAsync();
        return walk;
    }

    public async Task<Walk?> DeleteAsync(Guid id)
    {
        var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(walk => walk.Id == id);
        if (existingWalk == null)
        {
            return null;
        }

        dbContext.Walks.Remove(existingWalk);
        await dbContext.SaveChangesAsync();
        return existingWalk;
    }

    public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
    {
        var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

        // Filtering
        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
        {
            if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = walks.Where(walk => walk.Name.Contains(filterQuery));
            }
        }

        // Sorting
        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(walk => walk.Name) : walks.OrderByDescending(walk => walk.Name);
            }
            else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
            {
                walks = isAscending ? walks.OrderBy(walk => walk.Name) : walks.OrderByDescending(walk => walk.Name);
            }
        }

        // Pagination
        var skipResults = (pageNumber - 1) * pageSize;

        return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
        // return await walks.ToListAsync();
        // return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
    }

    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        return await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(walk => walk.Id == id);
    }

    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        var existingWalk = await dbContext.Walks.FirstOrDefaultAsync(walk => walk.Id == id);

        if (existingWalk == null)
        {
            return null;
        }

        existingWalk.Name = walk.Name;
        existingWalk.Description = walk.Description;
        existingWalk.WalkImageUrl = walk.WalkImageUrl;
        existingWalk.DifficultyId = walk.DifficultyId;
        existingWalk.RegionId = walk.RegionId;
        existingWalk.LengthInKm = walk.LengthInKm;

        await dbContext.SaveChangesAsync();

        return existingWalk;
    }
}