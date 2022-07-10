using HuskyEngine.Api.Dto.Response.Check;
using HuskyEngine.Engine;
using HuskyEngine.Engine.Parser.Errors;
using Microsoft.AspNetCore.Mvc;

namespace HuskyEngine.Api.Controllers;

[ApiController]
[Route("/api/check")]
public class CheckController : ControllerBase
{
    private readonly ILogger<CheckController> _logger;
    private readonly EvaluatorFactory _evaluator;

    public CheckController(ILogger<CheckController> logger, EvaluatorFactory evaluator)
    {
        _logger = logger;
        _evaluator = evaluator;
    }

    [HttpGet]
    public CheckResultDto Check(string formula)
    {
        var runtime = _evaluator.At(DateTime.Today);

        try
        {
            runtime.Parse(formula);

            _logger.LogInformation("Check formula '{Formula}' success", formula);

            return new CheckResultDto { Errors = new() };
        }
        catch (ParsingError e)
        {
            _logger.LogInformation(
                "Error checking formula '{Formula}', {Error}",
                formula, e.ToString());

            return new CheckResultDto
            {
                Errors = new List<CheckResultDto.Item>
                {
                    new()
                    {
                        Line = e.Line,
                        Position = e.Column,
                        Error = e.Message
                    }
                }
            };
        }
    }

}