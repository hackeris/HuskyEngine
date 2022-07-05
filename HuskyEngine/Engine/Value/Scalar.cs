using HuskyEngine.Engine.Runtime;
using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Value;

public class Scalar : IValue
{
    public Scalar(int value)
    {
        Value = value;
        Type = new ScalarType(PrimitiveType.Integer);
    }

    public Scalar(float value)
    {
        Value = value;
        Type = new ScalarType(PrimitiveType.Number);
    }

    public Scalar(bool value)
    {
        Value = value;
        Type = new ScalarType(PrimitiveType.Boolean);
    }

    public object Value { get; }
    public IType Type { get; }
}