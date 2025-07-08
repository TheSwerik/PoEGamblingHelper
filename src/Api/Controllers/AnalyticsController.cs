using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoEGamblingHelper.Application.Repositories;
using PoEGamblingHelper.Application.Services;
using PoEGamblingHelper.Domain.Entity.Analytics;

namespace PoEGamblingHelper.Api.Controllers;

public class AnalyticsController(IAnalyticsDayRepository analyticsRepository, IConfiguration configuration, IDateTimeService dateTimeService)
    : ApiControllerBase
{
    [HttpGet]
    [Authorize]
    public IAsyncEnumerable<AnalyticsDay> Get([FromQuery] DateOnly? start, [FromQuery] DateOnly? end)
    {
        if (start is null || end is null) return analyticsRepository.GetAll();

        return analyticsRepository.Get(start.Value, end.Value);
    }

    [HttpGet("check")]
    [Authorize]
    public Task<bool> Check()
    {
        return Task.FromResult(true);
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login([FromHeader(Name = "Authorization")] string apiKey)
    {
        var apiKeySolution = configuration.GetValue<string>("ApiKey");
        if (apiKey.Length <= "Bearer ".Length || !string.Equals(apiKey["Bearer ".Length..], apiKeySolution)) return Unauthorized();

        var claimsIdentity = new ClaimsIdentity(
            [new Claim(ClaimTypes.Name, "ApiKeyUser")],
            CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = dateTimeService.UtcNow().AddDays(30),
            AllowRefresh = true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Ok();
    }
}