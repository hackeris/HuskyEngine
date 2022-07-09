using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Lib.Function.Util;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Function;

public class Rank : IFunction
{
    private Rank(List<IType> argTypes, IType type)
    {
        Type = type;
        ArgTypes = argTypes;
    }

    public IValue Call(IRuntime runtime, List<IExpression> arguments)
    {
        if (arguments.Count == 1)
        {
            return RankVector((Vector)runtime.Eval(arguments[0]));
        }

        var masked = Mask.Apply(
            (Vector)runtime.Eval(arguments[0]),
            (Vector)runtime.Eval(arguments[1])
        );
        return RankVector(masked);
    }

    private static Vector RankVector(Vector vector)
    {
        return vector.ElementType switch
        {
            PrimitiveType.Integer =>
                RankValues(vector.AsInteger()),
            PrimitiveType.Number =>
                RankValues(vector.AsNumber()),
            _ => throw new Exception("unsupported")
        };
    }

    private static Vector RankValues<T>(Dictionary<string, T> values) where T : IComparable<T>
    {
        var sorted = values.ToList();
        sorted.Sort((x, y) => x.Value.CompareTo(y.Value));

        var ranks = sorted
            .Select((pair, i) => new KeyValuePair<string, int>(pair.Key, i + 1))
            .ToDictionary(p => p.Key, p => p.Value);
        return new Vector(ranks);
    }

    public static List<IFunction.Def> GetDefines()
    {
        var intVector = new VectorType(PrimitiveType.Integer);
        var numberVector = new VectorType(PrimitiveType.Number);

        var boolVector = new VectorType(PrimitiveType.Boolean);

        var functions = new List<Rank>
        {
            new(new List<IType> { intVector }, intVector),
            new(new List<IType> { numberVector }, intVector),
            new(new List<IType> { intVector, boolVector }, intVector),
            new(new List<IType> { numberVector, boolVector }, intVector),
        };

        return functions
            .Select(f => new IFunction.Def { Name = "rank", Function = f })
            .ToList();
    }

    public IType Type { get; }
    public List<IType> ArgTypes { get; }
}