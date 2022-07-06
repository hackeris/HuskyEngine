using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Runtime;

public interface IVector : IValue
{
    public PrimitiveType ElementType { get; }
}