using HuskyEngine.Engine.Runtime;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Context;

public interface IFunction
{
    public IValue Call(IRuntime runtime, List<IExpression> arguments);
    public IType Type { get; }
}