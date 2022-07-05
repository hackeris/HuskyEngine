using HuskyEngine.Engine.Runtime;
using HuskyEngine.Engine.Semantic;

namespace HuskyEngine.Engine.Context;

public interface IRuntime
{
    public IValue Eval(IExpression expression);
}