using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Lib.Function.Util;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Function;

public class ZScore : IFunction
{
    private ZScore(List<IType> argTypes, IType type)
    {
        Type = type;
        ArgTypes = argTypes;
    }

    public IValue Call(IRuntime runtime, List<IExpression> arguments)
    {
        if (arguments.Count == 1)
        {
            return ZScoreVector((Vector)runtime.Eval(arguments[0]));
        }

        var masked = Mask.Apply(
            (Vector)runtime.Eval(arguments[0]),
            (Vector)runtime.Eval(arguments[1])
        );
        return ZScoreVector(masked);
    }

    private static Vector ZScoreVector(Vector vector)
    {
        return vector.ElementType switch
        {
            PrimitiveType.Integer =>
                new Vector(ZScoreValues(vector.AsInteger())),
            PrimitiveType.Number =>
                new Vector(ZScoreValues(vector.AsNumber())),
            _ => throw new Exception("unsupported")
        };
    }

    private static Dictionary<string, float> ZScoreValues(Dictionary<string, int> values)
    {
        return ZScoreValues(
            values.ToDictionary(p => p.Key,
                p => (float)p.Value)
        );
    }

    private static Dictionary<string, float> ZScoreValues(Dictionary<string, float> values)
    {
        if (values.Count <= 1)
        {
            return values.ToDictionary(p => p.Key, _ => 0.0f);
        }

        var avg = values.Values.Aggregate(0.0f, (a, b) => a + b) / values.Count;
        var squareVar = values.Values
            .Select(a => a - avg)
            .Select(a => a * a)
            .Aggregate(0.0f, (a, b) => a + b) / (values.Count - 1);
        var std = (float)Math.Sqrt(squareVar);

        if (std <= 0.0)
        {
            return values.ToDictionary(p => p.Key, _ => 0.0f);
        }

        return values.ToDictionary(
            p => p.Key,
            p => (p.Value - avg) / std
        );
    }

    public static List<IFunction.Def> GetDefines()
    {
        var intVector = new VectorType(PrimitiveType.Integer);
        var numberVector = new VectorType(PrimitiveType.Number);

        var boolVector = new VectorType(PrimitiveType.Boolean);

        var functions = new List<ZScore>
        {
            new(new List<IType> { intVector }, numberVector),
            new(new List<IType> { numberVector }, numberVector),
            new(new List<IType> { intVector, boolVector }, numberVector),
            new(new List<IType> { numberVector, boolVector }, numberVector),
        };

        return functions
            .Select(f => new IFunction.Def { Name = "zscore", Function = f })
            .ToList();
    }

    public IType Type { get; }
    public List<IType> ArgTypes { get; }
}