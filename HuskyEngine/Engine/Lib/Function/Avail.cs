using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Function;

public class Avail : IFunction
{
    private Avail(PrimitiveType elementType, PrimitiveType defaultType)
    {
        Type = new VectorType(elementType);
        ArgTypes = new List<IType> { Type, new ScalarType(defaultType) };
    }

    public IValue Call(IRuntime runtime, List<IExpression> arguments)
    {
        var vector = (Vector)runtime.Eval(arguments[0]);
        var defaultValue = ((Scalar)runtime.Eval(arguments[1])).CastTo(vector.ElementType);

        var zeros = Zeros(runtime);

        return new Vector(vector.ElementType,
            zeros.Values.Keys
                .ToDictionary(key => key,
                    key => vector.Values.GetValueOrDefault(key, defaultValue.Value))
        );
    }

    private static Vector Zeros(IRuntime runtime)
    {
        var zero = new Identifier("zero", new VectorType(PrimitiveType.Number));
        return (Vector)runtime.Eval(zero);
    }

    public static List<IFunction.Def> GetDefines()
    {
        var availDefines = new List<Avail>
        {
            new(PrimitiveType.Integer, PrimitiveType.Integer),
            new(PrimitiveType.Number, PrimitiveType.Number),
            new(PrimitiveType.Number, PrimitiveType.Integer),
            new(PrimitiveType.Integer, PrimitiveType.Number),
        };
        return availDefines
            .Select(func => new IFunction.Def { Name = "avail", Function = func })
            .ToList();
    }

    public IType Type { get; }
    public List<IType> ArgTypes { get; }
}