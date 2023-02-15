using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Shared.Entity;
using Shared.QueryParameters;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class GemController : ControllerBase
{
    private readonly IGemDataRepository _gemDataRepository;

    public GemController(IGemDataRepository gemDataRepository) { _gemDataRepository = gemDataRepository; }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public Page<GemData> GetAll([FromQuery] GemDataQuery? query, [FromQuery] PageRequest page)
    {
        return _gemDataRepository.GetAll(query, page);
    }
}