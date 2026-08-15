using InfotecsTestApplication.Models.Entity;
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
    public async Task<ActionResult<List<ValueModel>>> GetValues([FromQuery]string file)
    {
        var result = await _valuesService.GetValuesFromFile(file);
        return Ok(result);
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        await _valuesService.SaveValuesFromFile(file);
        return Ok("File processed!");
    }
}