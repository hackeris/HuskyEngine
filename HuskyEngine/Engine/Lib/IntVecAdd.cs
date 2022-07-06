using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib;

public class IntVecAdd : IFunction
{
    public IValue Call(IRuntime runtime, List<IExpression> arguments)
    {
        var left = (IntVector)runtime.Eval(arguments[0]);
        var right = (IntVector)runtime.Eval(arguments[1]);

        var leftValues = left.Values;
        var rightValues = right.Values;
        var keys = left.Values.Keys.Union(right.Values.Keys);

        var values = from key in keys
            where leftValues.ContainsKey(key) && rightValues.ContainsKey(key)
            select new KeyValuePair<string, int>(key, leftValues[key] + rightValues[key]);

        return new IntVector { Values = new Dictionary<string, int>(values) };
    }

    public IType Type => new VectorType(PrimitiveType.Integer);

    public List<IType> ArgTypes => new()
    {
        new VectorType(PrimitiveType.Integer),
        new VectorType(PrimitiveType.Integer)
    };
}