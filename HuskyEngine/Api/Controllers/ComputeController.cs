using System.Diagnostics;
using HuskyEngine.Api.Dto;
using HuskyEngine.Api.Dto.Request;
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

    [HttpGet]
    public ComputeResult Compute(string formula, DateTime date)
    {
        return Compute(formula, date);
    }

    private ComputeResult Compute(string formula, DateTime date, bool boolAsNumber)
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

        var rawValue = value switch
        {
            Scalar s => s.Value,
            Vector v => (boolAsNumber, v.ElementType) switch
            {
                (true, PrimitiveType.Boolean) =>
                    v.AsBoolean()
                        .ToDictionary(p => p.Key,
                            p => (object)(p.Value ? 1 : 0)),
                _ => v.Values
            },
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

    [HttpPost]
    public ComputeResult Compute(ComputeRequest request)
    {
        return Compute(request.Formula, request.Date, request.BoolAsNumber);
    }
}