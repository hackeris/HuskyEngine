using HuskyEngine.Api.Dto;
using HuskyEngine.Data;
using Microsoft.AspNetCore.Mvc;

namespace HuskyEngine.Api.Controllers;

[ApiController]
[Route("/api/check")]
public class CheckController : ControllerBase
{
    private readonly ILogger<CheckController> _logger;
    private readonly DataSourceFactory _sourceFactoryFactory;

    public CheckController(ILogger<CheckController> logger, DataSourceFactory sourceFactory)
    {
        _logger = logger;
        _sourceFactoryFactory = sourceFactory;
    }

    [HttpGet(Name = "check")]
    public CheckResult Check(string code)
    {
        return new CheckResult { };
    }
}