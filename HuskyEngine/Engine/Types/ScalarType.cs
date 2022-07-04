namespace HuskyEngine.Engine.Types;

public class ScalarType : IType
{
    public ScalarType(PrimitiveType type)
    {
        Type = type;
    }

    public PrimitiveType Type { get; }
}