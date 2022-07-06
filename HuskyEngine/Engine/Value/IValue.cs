using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Value;

public interface IValue
{
    public IType Type { get; }
}