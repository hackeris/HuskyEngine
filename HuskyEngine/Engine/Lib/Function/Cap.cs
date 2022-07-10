using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Function;

public class Cap : IFunction
{
    private Cap(PrimitiveType valueType)
    {
        Type = new VectorType(valueType);
        ArgTypes = new List<IType> { Type, new ScalarType(valueType), new ScalarType(valueType) };
    }

    public IValue Call(IRuntime runtime, List<IExpression> arguments)
    {
        var vector = (Vector)runtime.Eval(arguments[0]);
        var min = (Scalar)runtime.Eval(arguments[1]);
        var max = (Scalar)runtime.Eval(arguments[2]);
        return CapVector(vector, min, max);
    }

    public static IValue CapVector(Vector vector, Scalar maxAbs)
    {
        var min = maxAbs switch
        {
            { ValueType: PrimitiveType.Integer, Value: var value } =>
                new Scalar(-(int)value),
            { ValueType: PrimitiveType.Number, Value: var value } =>
                new Scalar(-(float)value),
            _ => throw new Exception("Unsupported")
        };
        return CapVector(vector, min, maxAbs);
    }

    private static IValue CapVector(Vector vector, Scalar min, Scalar max)
    {
        return vector.Type switch
        {
            VectorType { ElementType: PrimitiveType.Integer } =>
                new Vector(CapVector(
                    vector.AsInteger(),
                    min.CastTo(PrimitiveType.Integer).AsInteger(),
                    max.CastTo(PrimitiveType.Integer).AsInteger()
                )),
            VectorType { ElementType: PrimitiveType.Number } =>
                new Vector(CapVector(
                    vector.AsNumber(),
                    min.CastTo(PrimitiveType.Number).AsNumber(),
                    max.CastTo(PrimitiveType.Number).AsNumber()
                )),
            _ => throw new Exception("Unsupported")
        };
    }

    private static Dictionary<string, T> CapVector<T>(Dictionary<string, T> values, T min, T max)
        where T : IComparable<T>
    {
        return values
            .ToDictionary(p => p.Key,
                p =>
                {
                    if (p.Value.CompareTo(max) > 0)
                    {
                        return max;
                    }

                    if (p.Value.CompareTo(min) < 0)
                    {
                        return min;
                    }

                    return p.Value;
                });
    }

    public static List<IFunction.Def> GetDefines()
    {
        var functions = new List<Cap>
        {
            new(PrimitiveType.Integer),
            new(PrimitiveType.Number),
        };

        return functions
            .Select(f => new IFunction.Def { Name = "cap", Function = f })
            .ToList();
    }

    public IType Type { get; }
    public List<IType> ArgTypes { get; }
}