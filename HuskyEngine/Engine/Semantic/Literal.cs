using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Semantic;

public class Literal : IExpression
{
    public Literal(int value)
    {
        Value = value;
        Type = new ScalarType(PrimitiveType.Integer);
    }

    public Literal(float value)
    {
        Value = value;
        Type = new ScalarType(PrimitiveType.Number);
    }

    public object Value { get; }
    public IType Type { get; }
}