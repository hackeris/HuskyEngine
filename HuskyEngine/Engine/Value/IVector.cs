using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Runtime;

public interface IVector : IValue
{
    public PrimitiveType ElementType { get; }
}