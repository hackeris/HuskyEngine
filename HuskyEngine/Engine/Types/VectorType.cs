namespace HuskyEngine.Engine.Types;

public class VectorType : IType
{
    public VectorType(PrimitiveType elementType)
    {
        ElementType = elementType;
    }

    public PrimitiveType ElementType { get; }
}