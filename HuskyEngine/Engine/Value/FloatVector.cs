using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Runtime;

public class FloatVector : IVector
{
    public Dictionary<string, float> Values;

    public PrimitiveType ElementType => PrimitiveType.Number;

    public IType Type => new VectorType(PrimitiveType.Number);
}