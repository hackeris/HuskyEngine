namespace HuskyEngine.Engine.Types;

public class VectorType : IType
{
    protected bool Equals(VectorType other)
    {
        return ElementType == other.ElementType;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((VectorType)obj);
    }

    public override int GetHashCode()
    {
        return (int)ElementType;
    }

    public VectorType(PrimitiveType elementType)
    {
        ElementType = elementType;
    }

    public PrimitiveType ElementType { get; }
}