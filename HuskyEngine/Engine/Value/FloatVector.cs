using HuskyEngine.Engine.Runtime;
using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Value;

public class FloatVector : IVector
{
    public Dictionary<string, float> Values;

    public PrimitiveType ElementType => PrimitiveType.Number;

    public IType Type => new VectorType(PrimitiveType.Number);
}