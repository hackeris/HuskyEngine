using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Runtime;

public class BoolVector : IVector
{
    public Dictionary<string, bool> Values;

    public PrimitiveType ElementType => PrimitiveType.Boolean;

    public IType Type => new VectorType(PrimitiveType.Boolean);
}