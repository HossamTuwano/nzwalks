using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.Domain;

namespace NZWalks.Controllers;

[ApiController]
[Route("api/[controller]")]
class RegionsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var regions = new List<Region> {
            new Region {
                Id = Guid.NewGuid(),
                Name = "Land of OO",
                Code = "AT",
                RegionImageUrl = "https://mir-s3-cdn-cf.behance.net/project_modules/max_1200/0374f040330989.639c9cbb8e0ec.jpg"
            },
            new Region {
                Id = Guid.NewGuid(),
                Name = "Asgard",
                Code = "AG",
                RegionImageUrl = "https://en.wikipedia.org/wiki/Asgard#/media/File:Asgard_and_Bifrost_in_interpretation_of_Otto_Schenk_in_Wagner's_Das_Rheingold.jpg"
            },
            new Region {
                Id = Guid.NewGuid(),
                Name = "Land of OO",
                Code = "AT",
                RegionImageUrl = "https://marvel.fandom.com/wiki/Knowhere?file=Knowhere_from_Infinity_Countdown_Vol_1_4_001.jpg"
            }
        };

        return Ok(regions);
    }
}