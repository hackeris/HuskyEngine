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
}