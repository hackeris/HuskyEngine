using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Value;

public class Scalar : IValue
{
    public Scalar(int value)
    {
        Value = value;
        ValueType = PrimitiveType.Integer;
    }

    public Scalar(float value)
    {
        Value = value;
        ValueType = PrimitiveType.Number;
    }

    public Scalar(bool value)
    {
        Value = value;
        ValueType = PrimitiveType.Boolean;
    }

    public Scalar(PrimitiveType type, object value)
    {
        Value = value;
        ValueType = type;
    }

    public object Value { get; }

    public IType Type => new ScalarType(ValueType);

    public PrimitiveType ValueType { get; }

    public int AsInteger()
    {
        return (int)Value;
    }

    public float AsNumber()
    {
        return (float)Value;
    }

    public Scalar CastTo(PrimitiveType type)
    {
        if (ValueType == type)
        {
            return this;
        }

        return (ValueType, type) switch
        {
            (PrimitiveType.Integer, PrimitiveType.Number) =>
                new Scalar((float)(int)Value),
            (PrimitiveType.Number, PrimitiveType.Integer) =>
                new Scalar((int)(float)Value),
            _ => throw new Exception($"Cast {ValueType} to {type} is not supported")
        };
    }
}