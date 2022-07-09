using HuskyEngine.Api.Dto;
using HuskyEngine.Data;
using Microsoft.AspNetCore.Mvc;

namespace HuskyEngine.Api.Controllers;

[ApiController]
[Route("/api/check")]
public class CheckApiController : ControllerBase
{
    private readonly ILogger<CheckApiController> _logger;
    private readonly DataSourceFactory _sourceFactoryFactory;

    public CheckApiController(ILogger<CheckApiController> logger, DataSourceFactory sourceFactory)
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