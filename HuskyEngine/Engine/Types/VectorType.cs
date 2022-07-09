namespace HuskyEngine.Engine.Types;

public class VectorType : IType
{
    private bool Equals(VectorType other)
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

    public override string ToString()
    {
        var eType = ElementType switch
        {
            PrimitiveType.Integer => "int",
            PrimitiveType.Number => "number",
            PrimitiveType.Boolean => "bool",
            _ => "unknown"
        };
        return $"[{eType}]";
    }
}