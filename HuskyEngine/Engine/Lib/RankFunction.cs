using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib;

public class RankFunction : IFunction
{
    public IValue Call(IRuntime runtime, List<IExpression> arguments)
    {
        throw new NotImplementedException();
    }

    public IType Type => new VectorType(PrimitiveType.Integer);

    public List<IType> ArgTypes => new()
    {
        new VectorType(PrimitiveType.Number)
    };
}