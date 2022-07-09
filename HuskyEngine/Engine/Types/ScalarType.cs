namespace HuskyEngine.Engine.Types;

public class ScalarType : IType
{
    private bool Equals(ScalarType other)
    {
        return Type == other.Type;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ScalarType)obj);
    }

    public override int GetHashCode()
    {
        return (int)Type;
    }

    public ScalarType(PrimitiveType type)
    {
        Type = type;
    }

    public PrimitiveType Type { get; }

    public override string ToString()
    {
        return Type switch
        {
            PrimitiveType.Integer => "int",
            PrimitiveType.Number => "number",
            PrimitiveType.Boolean => "bool",
            _ => "unknown"
        };
    }
}