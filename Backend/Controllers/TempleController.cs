using Backend.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Shared.Entity;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class TempleController : ControllerBase
{
    private readonly IRepository<TempleCost, Guid> _templeCostRepository;

    public TempleController(IRepository<TempleCost, Guid> templeCostRepository)
    {
        _templeCostRepository = templeCostRepository;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public TempleCost Get()
    {
        return _templeCostRepository.GetAll().FirstOrDefault() ?? throw new NoTempleDataException();
    }
}