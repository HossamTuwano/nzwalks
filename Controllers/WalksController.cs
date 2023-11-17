using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.CustomActionFilter;
using NZWalks.Data;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers;

[ApiController]
[Route("api/[controller]")]

public class WalksController : ControllerBase
{
    private readonly IWalkRepository walkRepository;

    public WalksController(IWalkRepository walkRepository)
    {
        this.walkRepository = walkRepository;
    }

    // CREATE Walk
    // POST: api/Walks
    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
    {

        // Map AddWalkDto to Domain Modal
        var walkRequestDomainModel = new Walk
        {
            Description = addWalkRequestDto.Description,
            LengthInKm = addWalkRequestDto.LengthInKm,
            Name = addWalkRequestDto.Name,
            WalkImageUrl = addWalkRequestDto.WalkImageUrl,
            DifficultyId = addWalkRequestDto.DifficultyId,
            RegionId = addWalkRequestDto.RegionId,
        };

        await walkRepository.CreateAsync(walkRequestDomainModel);


        // Converting Back to Dto
        var walkDto = new WalkDto
        {
            Description = walkRequestDomainModel.Description,
            RegionId = walkRequestDomainModel.RegionId,
            DifficultyId = walkRequestDomainModel.DifficultyId,
            Id = walkRequestDomainModel.Id,
            LengthInKm = walkRequestDomainModel.LengthInKm,
            Name = walkRequestDomainModel.Name,
            WalkImageUrl = walkRequestDomainModel.WalkImageUrl,
        };

        return Ok(walkDto);


    }

    // Get Walks
    // GET: api/walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=True&pageNumber=1&pageSize=10
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
    {
        var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);
        var walkDto = new List<WalkDto>();

        foreach (var walkDomainModel in walksDomainModel)
        {
            walkDto.Add(new WalkDto()
            {
                Description = walkDomainModel.Description,
                DifficultyId = walkDomainModel.DifficultyId,
                RegionId = walkDomainModel.RegionId,
                Id = walkDomainModel.Id,
                LengthInKm = walkDomainModel.LengthInKm,
                Name = walkDomainModel.Name,
                WalkImageUrl = walkDomainModel.WalkImageUrl,

            }
            );
        }

        return Ok(walkDto);
    }

    [HttpGet]
    [Route("{id:Guid}")]

    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var walkDomainModel = await walkRepository.GetByIdAsync(id);

        // Checks if exists
        if (walkDomainModel == null)
        {
            return NotFound();
        }

        // Map domain modengiril to dto

        var walkDto = new WalkDto
        {
            Description = walkDomainModel.Description,
            Id = walkDomainModel.Id,
            DifficultyId = walkDomainModel.DifficultyId,
            LengthInKm = walkDomainModel.LengthInKm,
            Name = walkDomainModel.Name,
            RegionId = walkDomainModel.RegionId,
            WalkImageUrl = walkDomainModel.WalkImageUrl,
        };

        return Ok(walkDto);
    }
    [HttpPut]
    [Route("{id:Guid}")]
    [ValidateModel]
    public async Task<IActionResult> Update([FromRoute] Guid id, Walk updateWalkRequestDto)
    {
        var walkDomainModel = new Walk
        {
            Description = updateWalkRequestDto.Description,
            DifficultyId = updateWalkRequestDto.DifficultyId,
            LengthInKm = updateWalkRequestDto.LengthInKm,
            Name = updateWalkRequestDto.Name,
            WalkImageUrl = updateWalkRequestDto.WalkImageUrl,
            RegionId = updateWalkRequestDto.RegionId,

        };

        var walkDomainModelRepo = await walkRepository.UpdateAsync(id, walkDomainModel);

        if (walkDomainModelRepo == null)
        {
            return NotFound();
        }

        var walkDtoFinal = new WalkDto
        {
            Description = walkDomainModel.Description,
            DifficultyId = walkDomainModel.DifficultyId,
            LengthInKm = walkDomainModel.LengthInKm,
            Name = walkDomainModel.Name,
            WalkImageUrl = walkDomainModel.WalkImageUrl,

        };

        return Ok(walkDtoFinal);
    }
    [HttpDelete]
    [Route("{id:Guid}")]

    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deletedWalkModel = await walkRepository.DeleteAsync(id);

        if (deletedWalkModel == null)
        {
            return NotFound();
        }

        var deleteWalkDto = new WalkDto
        {
            Id = deletedWalkModel.Id,
            Description = deletedWalkModel.Description,
            DifficultyId = deletedWalkModel.DifficultyId,
            LengthInKm = deletedWalkModel.LengthInKm,
            Name = deletedWalkModel.LengthInKm,
            RegionId = deletedWalkModel.RegionId,
            WalkImageUrl = deletedWalkModel.WalkImageUrl,
        };

        return Ok(deleteWalkDto);
    }


}