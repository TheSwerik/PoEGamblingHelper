using Application.Services;
using Domain.Entity;
using Domain.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class TempleController : ApiControllerBase
{
    private readonly IApplicationDbContext _applicationDbContext;

    public TempleController(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    [HttpGet]
    [OutputCache(PolicyName = "FetchData")]
    public TempleCost Get()
    {
        return _applicationDbContext.TempleCost.OrderByDescending(cost => cost.TimeStamp).FirstOrDefault()
               ?? throw new NoTempleDataException();
    }
}