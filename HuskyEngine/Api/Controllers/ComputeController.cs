using System.Diagnostics;
using HuskyEngine.Api.Dto;
using HuskyEngine.Engine;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;
using Microsoft.AspNetCore.Mvc;

namespace HuskyEngine.Api.Controllers;

[ApiController]
[Route("/api/compute")]
public class ComputeController : ControllerBase
{
    private readonly ILogger<ComputeController> _logger;
    private readonly EvaluatorFactory _evaluator;

    public ComputeController(ILogger<ComputeController> logger, EvaluatorFactory evaluatorFactory)
    {
        _logger = logger;
        _evaluator = evaluatorFactory;
    }

    [HttpGet(Name = "compute")]
    public ComputeResult Compute(string formula, DateTime date)
    {
        var evaluator = _evaluator.At(date);

        var sw = Stopwatch.StartNew();
        var value = evaluator.Eval(formula);

        _logger.LogInformation(
            "Evaluate formula {Formula} at {Date}, time elapsed {Elapsed}ms",
            formula, date, sw.Elapsed.TotalMilliseconds);

        var type = value.Type switch
        {
            ScalarType { Type: PrimitiveType.Integer } => "int",
            ScalarType { Type: PrimitiveType.Number } => "float",
            ScalarType { Type: PrimitiveType.Boolean } => "bool",
            VectorType { ElementType: PrimitiveType.Integer } => "vector<int>",
            VectorType { ElementType: PrimitiveType.Number } => "vector<float>",
            VectorType { ElementType: PrimitiveType.Boolean } => "vector<bool>",
            _ => throw new Exception($"Unexpected value type {value.Type}")
        };

        var rawValue = value.Type switch
        {
            ScalarType => ((Scalar)value).Value,
            VectorType => ((Vector)value).Values,
            _ => throw new Exception($"Unexpected value type {value.Type}")
        };

        return new ComputeResult
        {
            Date = date.ToString("yyyy-MM-dd"),
            Formula = formula,
            Type = type,
            Value = rawValue
        };
    }
}