using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Lib.Function.Util;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Function;

public class Sum : IFunction
{
    private Sum(List<IType> argTypes, IType type)
    {
        Type = type;
        ArgTypes = argTypes;
    }

    public IValue Call(IRuntime runtime, List<IExpression> arguments)
    {
        if (arguments.Count == 1)
        {
            return SumVector((Vector)runtime.Eval(arguments[0]));
        }

        var masked = Mask.Apply(
            (Vector)runtime.Eval(arguments[0]),
            (Vector)runtime.Eval(arguments[1])
        );
        return SumVector(masked);
    }

    private static Scalar SumVector(Vector vector)
    {
        return vector.ElementType switch
        {
            PrimitiveType.Integer =>
                new Scalar(vector.AsInteger().Values.Aggregate(0, (a, b) => a + b)),
            PrimitiveType.Number =>
                new Scalar(vector.AsNumber().Values.Aggregate(0.0f, (a, b) => a + b)),
            _ => throw new Exception("unsupported")
        };
    }

    public static List<IFunction.Def> GetDefines()
    {
        var intVector = new VectorType(PrimitiveType.Integer);
        var numberVector = new VectorType(PrimitiveType.Number);
        var intScalar = new ScalarType(PrimitiveType.Integer);
        var numberScalar = new ScalarType(PrimitiveType.Number);

        var boolVector = new VectorType(PrimitiveType.Boolean);

        var functions = new List<Sum>
        {
            new(new List<IType> { intVector }, intScalar),
            new(new List<IType> { numberVector }, numberScalar),
            new(new List<IType> { intVector, boolVector }, intScalar),
            new(new List<IType> { numberVector, boolVector }, numberScalar),
        };

        return functions
            .Select(f => new IFunction.Def { Name = "sum", Function = f })
            .ToList();
    }

    public IType Type { get; }
    public List<IType> ArgTypes { get; }
}