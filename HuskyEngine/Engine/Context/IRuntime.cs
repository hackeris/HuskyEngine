using HuskyEngine.Engine.Runtime;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Context;

public interface IRuntime
{
    public IValue Eval(IExpression expression);

    public IPredefine GetPredefine();
}