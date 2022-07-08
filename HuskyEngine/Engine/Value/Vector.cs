using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Value;

public class Vector : IValue
{
    public readonly Dictionary<string, object> Values;

    public Vector(Dictionary<string, float> values)
    {
        Values = values.ToDictionary(p => p.Key,
            p => (object)p.Value);
        ElementType = PrimitiveType.Number;
    }

    public Vector(Dictionary<string, int> values)
    {
        Values = values.ToDictionary(p => p.Key,
            p => (object)p.Value);
        ElementType = PrimitiveType.Integer;
    }

    public Vector(Dictionary<string, bool> values)
    {
        Values = values.ToDictionary(p => p.Key,
            p => (object)p.Value);
        ElementType = PrimitiveType.Boolean;
    }

    public Vector(PrimitiveType type, Dictionary<string, object> values)
    {
        Values = values;
        ElementType = type;
    }

    public Vector(ICollection<string> keys, Scalar value)
    {
        ElementType = value.ValueType;
        Values = keys.ToDictionary(key => key, _ => value.Value);
    }

    public PrimitiveType ElementType { get; }

    public IType Type => new VectorType(ElementType);
}