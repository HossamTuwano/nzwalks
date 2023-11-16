using Microsoft.AspNetCore.Mvc;

namespace NZWalks.Controllers;

[ApiController]
[Route("api/[controller]")]

public class StudentController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllStudent()
    {
        string[] studentNames = new string[] { "Hossam", "Nana" };

        return Ok(studentNames);
    }
}