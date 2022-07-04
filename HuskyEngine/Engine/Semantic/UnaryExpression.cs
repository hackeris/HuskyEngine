using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Semantic;

public class UnaryExpression : IExpression
{
    public Operation.Unary Operator { get; set; }
    public IExpression Operand { get; set; }
    public IType Type { get; set; }
}