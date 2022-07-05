using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Runtime;

public class IntVector : IVector
{
    public Dictionary<string, int> Values;

    public PrimitiveType ElementType => PrimitiveType.Integer;

    public IType Type => new VectorType(PrimitiveType.Integer);
}