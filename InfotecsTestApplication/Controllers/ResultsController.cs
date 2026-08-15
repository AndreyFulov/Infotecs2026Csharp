using InfotecsTestApplication.DTO;
using InfotecsTestApplication.Models.Entity;
using InfotecsTestApplication.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InfotecsTestApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultsController : ControllerBase
{
    private readonly IResultService _rsService;

    public ResultsController(IResultService rsService)
    {
        _rsService = rsService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ResultResponseDTO>>> GetResults([FromQuery] ResultFilter filter)
    {
        var results = await _rsService.GetResults(filter);
        return Ok(results);
    }
}