using HuskyEngine.Api.Dto;
using HuskyEngine.Data;
using Microsoft.AspNetCore.Mvc;

namespace HuskyEngine.Api.Controllers;

[ApiController]
[Route("/api/compute")]
public class ComputeApiController : ControllerBase
{
    private readonly ILogger<ComputeApiController> _logger;
    private readonly DataSourceFactory _sourceFactoryFactory;

    public ComputeApiController(ILogger<ComputeApiController> logger, DataSourceFactory sourceFactory)
    {
        _logger = logger;
        _sourceFactoryFactory = sourceFactory;
    }

    [HttpGet(Name = "compute")]
    public ComputeResult Compute(string code, DateTime date)
    {
        var source = _sourceFactoryFactory.At(date);
        var values = source.GetVector(code, 0);

        var exist = source.Exist(code);

        _logger.LogInformation("factor exist {Code} {Exist} {Date}", code, exist, date);

        return new ComputeResult
        {
            Exist = exist,
            Values = values
                .ToDictionary(p => p.Key, p => (object)p.Value)
        };
    }
}