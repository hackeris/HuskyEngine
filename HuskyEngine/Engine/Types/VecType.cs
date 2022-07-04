namespace HuskyEngine.Engine.Types;

public class VecType : IType
{
    public VecType(PrimitiveType elementType)
    {
        ElementType = elementType;
    }

    public PrimitiveType ElementType { get; }
}