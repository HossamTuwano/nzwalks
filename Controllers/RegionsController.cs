using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Data;
using NZWalks.Data.DTO;
using NZWalks.Models.Domain;
using NZWalks.Models.DTO;

namespace NZWalks.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegionsController : ControllerBase
{
    private readonly NZWalksDbContext dbContext;

    public RegionsController(NZWalksDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // Gets all Regions
    // GET: https://localhost:{port}/api/regions
    [HttpGet]

    public IActionResult GetAll()
    {
        // gets data from Database
        var regionsDomain = dbContext.Regions.ToList();

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
    public IActionResult GetById([FromRoute] Guid id)
    {
        // Gets Data Domain from Database
        var regionDomain = dbContext.Regions.Find(id);

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

    // POST: https://localhost:{port}/api/regions
    [HttpPost]
    public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
    {
        // Maps/Convert Region Domain to Region Dto
        var regionDomainModel = new Region
        {
            Code = addRegionRequestDto.Code,
            Name = addRegionRequestDto.Name,
            RegionImageUrl = addRegionRequestDto.RegionImageUrl,
        };

        // Uses Domain Model to Create Region
        dbContext.Add(regionDomainModel);
        dbContext.SaveChanges();

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
    public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
    {
        // Check if Region Exits
        var regionDomainModel = dbContext.Regions.FirstOrDefault(region => region.Id == id);

        if (regionDomainModel == null)
        {
            return NotFound();
        }

        // Map Dto to Domain Model
        regionDomainModel.Code = updateRegionRequestDto.Code;
        regionDomainModel.Name = updateRegionRequestDto.Name;
        regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

        dbContext.SaveChanges();

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
    public IActionResult Delete([FromRoute] Guid id)
    {
        // Checks if Region exists
        var regionDomainModel = dbContext.Regions.FirstOrDefault(region => region.Id == id);
        if (regionDomainModel == null)
        {
            return NotFound();
        }

        // Delete Region
        dbContext.Regions.Remove(regionDomainModel);
        dbContext.SaveChanges();

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