using HuskyEngine.Api.Dto.Response.Health;
using HuskyEngine.Data.Cache;
using Microsoft.AspNetCore.Mvc;

namespace HuskyEngine.Api.Controllers;

[ApiController]
public class HealthController : ControllerBase
{
    private readonly DataCache _cache;

    public HealthController(DataCache cache)
    {
        _cache = cache;
    }

    [HttpGet]
    [Route("/api/health/cache")]
    public CacheStatusDto CheckCache()
    {
        return new CacheStatusDto
        {
            MaxSize = _cache.MaxSize(),
            Used = _cache.Used()
        };
    }
}