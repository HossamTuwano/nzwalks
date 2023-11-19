using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.CustomActionFilter;
using NZWalks.Data;
using NZWalks.Data.DTO;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly NZWalksDbContext dbContext;
    private readonly IRegionRepository regionRepository;

    public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
    {
        this.dbContext = dbContext;
        this.regionRepository = regionRepository;
    }

    // Gets all Regions
    // GET: https://localhost:{port}/api/regions
    [HttpGet]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetAll()
    {
        // gets data from Database
        var regionsDomain = await regionRepository.GetAllAsync();

        // Maps Models to DTO
        var regionsDto = new List<RegionDto>();

        foreach (var regionDomain in regionsDomain)
        {
            regionsDto.Add(new RegionDto()
            {
                Id = regionDomain.Id,
                Name = regionDomain.Name,
                Code = regionDomain.Code,
                RegionImageUrl = regionDomain.RegionImageUrl,
            }
            );
        }
        // returns DTO
        return Ok(regionsDto);
    }

    // Gets Region by Id
    // GET: https://localhost:{port}/api/regions/{id}
    [HttpGet]
    [Route("{id:Guid}")]
    [Authorize(Roles = "Reader")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        // Gets Data Domain from Database
        var regionDomain = await dbContext.Regions.FindAsync(id);

        // checks if region exists
        if (regionDomain == null)
        {
            return NotFound();
        }

        // Maps/Convert Region Model to RegionDto
        var regionDto = new RegionDto
        {
            Id = regionDomain.Id,
            Code = regionDomain.Code,
            Name = regionDomain.Name,
            RegionImageUrl = regionDomain.RegionImageUrl,
        };
        // Return Dto back to client
        return Ok(regionDto);
    }

    // Creating new region
    // POST: https://localhost:{port}/api/regions
    [HttpPost]
    [ValidateModel]
    [Authorize(Roles = "Writer")]

    public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {

        // Maps/Convert Region Domain to Region Dto
        var regionDomainModel = new Region
        {
            Code = addRegionRequestDto.Code,
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl,
        };

        // Uses Domain Model to Create Region
        await dbContext.AddAsync(regionDomainModel);
        await dbContext.SaveChangesAsync();

        // Map Domain Model to Dto

        var regionDto = new RegionDto
        {
            Code = regionDomainModel.Code,
            Id = regionDomainModel.Id,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl,
        };

        return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id }, regionDomainModel);


    }

    // Updates an Existing Region
    // PUT: https://localhost:{port}/api/regions/{ID}
    [HttpPut]
    [Route("{id:Guid}")]
    [Authorize(Roles = "Writer")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        // Check if Region Exits
        var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        // Map Dto to Domain Model
        regionDomainModel.Code = updateRegionRequestDto.Code;
        regionDomainModel.Name = updateRegionRequestDto.Name;
        regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

        await dbContext.SaveChangesAsync();

        // Convert Domain Model to Dto 
        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl,

        };

        return Ok(regionDto);
    }

    // Delete a Region from Database
    // DELETE: https://localhost:{port}/api/regions/{ID}
    [HttpDelete]
    [Route("{id:Guid}")]
    [Authorize(Roles = "Writer,Reader")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        // Checks if Region exists
        var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);
        if (regionDomainModel == null)
        {
            return NotFound();
        }

        // Delete Region
        dbContext.Regions.Remove(regionDomainModel);
        await dbContext.SaveChangesAsync();

        // Converts Domain back to DTO
        var regionDto = new RegionDto
        {
            Id = regionDomainModel.Id,
            Code = regionDomainModel.Code,
            Name = regionDomainModel.Name,
            RegionImageUrl = regionDomainModel.RegionImageUrl,

        };

        return Ok(regionDto);
    }


}

//  var region = new List<Region> {
//         new Region
//         {
//             Id = Guid.NewGuid(),
//             Name = "Land of OO",
//             Code = "AT",
//             RegionImageUrl = "https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/0374f040330989.639c9cbb8e0ec.jpg"
//         },
//         new Region
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Asgard",
//                 Code = "AG",
//                 RegionImageUrl = "https://en.wikipedia.org/wiki/Asgard#/media/File:Asgard_and_Bifrost_in_interpretation_of_Otto_Schenk_in_Wagner's_Das_Rheingold.jpg"
//             },
//             new Region
//             {
//                 Id = Guid.NewGuid(),
//                 Name = "Knowhere",
//                 Code = "KW",
//                 RegionImageUrl = "https://marvel.fandom.com/wiki/Knowhere?file=Knowhere_from_Infinity_Countdown_Vol_1_4_001.jpg"
//             }
//     };        }