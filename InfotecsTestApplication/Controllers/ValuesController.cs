using InfotecsTestApplication.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfotecsTestApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
    private readonly IValuesService _valuesService;
    public ValuesController(IValuesService valuesService)
    {
        _valuesService = valuesService;
    }
    [HttpGet]
    public IActionResult GetValues()
    {
        return Ok("Hello World!");
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        await _valuesService.SaveValuesFromFile(file);
        return Ok("File processed!");
    }
}