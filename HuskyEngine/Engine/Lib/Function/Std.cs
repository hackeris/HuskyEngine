using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Lib.Function.Util;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Function;

public class Std : IFunction
{
    private Std(List<IType> argTypes, IType type)
    {
        Type = type;
        ArgTypes = argTypes;
    }

    public IValue Call(IRuntime runtime, List<IExpression> arguments)
    {
        if (arguments.Count == 1)
        {
            return StdVector((Vector)runtime.Eval(arguments[0]));
        }

        var masked = Mask.Apply(
            (Vector)runtime.Eval(arguments[0]),
            (Vector)runtime.Eval(arguments[1])
        );
        return StdVector(masked);
    }

    private static Scalar StdVector(Vector vector)
    {
        return vector.ElementType switch
        {
            PrimitiveType.Integer =>
                new Scalar(StdValues(vector.AsInteger())),
            PrimitiveType.Number =>
                new Scalar(StdValues(vector.AsNumber())),
            _ => throw new Exception("unsupported")
        };
    }

    private static float StdValues(Dictionary<string, int> values)
    {
        if (values.Count <= 1)
        {
            return 0;
        }

        var avg = values.Values.Aggregate(0, (a, b) => a + b) / (float)values.Count;
        var squareVar = values.Values
            .Select(a => a - avg)
            .Select(a => a * a)
            .Aggregate(0.0f, (a, b) => a + b) / values.Count;
        return (float)Math.Sqrt(squareVar);
    }

    private static float StdValues(Dictionary<string, float> values)
    {
        if (values.Count <= 1)
        {
            return 0;
        }

        var avg = values.Values.Aggregate(0.0f, (a, b) => a + b) / values.Count;
        var squareVar = values.Values
            .Select(a => a - avg)
            .Select(a => a * a)
            .Aggregate(0.0f, (a, b) => a + b) / (values.Count - 1);
        return (float)Math.Sqrt(squareVar);
    }

    public static List<IFunction.Def> GetDefines()
    {
        var intVector = new VectorType(PrimitiveType.Integer);
        var numberVector = new VectorType(PrimitiveType.Number);
        var numberScalar = new ScalarType(PrimitiveType.Number);

        var boolVector = new VectorType(PrimitiveType.Boolean);

        var functions = new List<Std>
        {
            new(new List<IType> { intVector }, numberScalar),
            new(new List<IType> { numberVector }, numberScalar),
            new(new List<IType> { intVector, boolVector }, numberScalar),
            new(new List<IType> { numberVector, boolVector }, numberScalar),
        };

        return functions
            .Select(f => new IFunction.Def { Name = "std", Function = f })
            .ToList();
    }

    public IType Type { get; }
    public List<IType> ArgTypes { get; }
}