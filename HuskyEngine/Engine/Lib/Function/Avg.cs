using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Lib.Function.Util;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Function;

public class Avg : IFunction
{
    private Avg(List<IType> argTypes, IType type)
    {
        Type = type;
        ArgTypes = argTypes;
    }

    public IValue Call(IRuntime runtime, List<IExpression> arguments)
    {
        if (arguments.Count == 1)
        {
            return AvgVector((Vector)runtime.Eval(arguments[0]));
        }

        var masked = Mask.Apply(
            (Vector)runtime.Eval(arguments[0]),
            (Vector)runtime.Eval(arguments[1])
        );
        return AvgVector(masked);
    }

    private static Scalar AvgVector(Vector vector)
    {
        return vector.ElementType switch
        {
            PrimitiveType.Integer =>
                new Scalar(vector.AsInteger().Values.Aggregate(0, (a, b) => a + b) / (float)vector.Values.Count),
            PrimitiveType.Number =>
                new Scalar(vector.AsNumber().Values.Aggregate(0.0f, (a, b) => a + b) / vector.Values.Count),
            _ => throw new Exception("unsupported")
        };
    }

    public static List<IFunction.Def> GetDefines()
    {
        var intVector = new VectorType(PrimitiveType.Integer);
        var numberVector = new VectorType(PrimitiveType.Number);
        var numberScalar = new ScalarType(PrimitiveType.Number);

        var boolVector = new VectorType(PrimitiveType.Boolean);

        var functions = new List<Avg>
        {
            new(new List<IType> { intVector }, numberScalar),
            new(new List<IType> { numberVector }, numberScalar),
            new(new List<IType> { intVector, boolVector }, numberScalar),
            new(new List<IType> { numberVector, boolVector }, numberScalar),
        };

        return functions
            .Select(f => new IFunction.Def { Name = "avg", Function = f })
            .ToList();
    }

    public IType Type { get; }
    public List<IType> ArgTypes { get; }
}