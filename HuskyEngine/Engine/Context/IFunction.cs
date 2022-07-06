using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Context;

public interface IFunction
{
    public IValue Call(IRuntime runtime, List<IExpression> arguments);
    public IType Type { get; }
    public List<IType> ArgTypes { get; }

    public class Id
    {
        public string Name { get; set; }
        public List<IType> ArgTypes { get; set; }
    }
}