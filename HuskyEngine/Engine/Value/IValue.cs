using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Runtime;

public interface IValue
{
    public IType Type { get; }
}