using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Semantic;

public class BinaryExpression : IExpression
{
    public Operation.Binary Operator { get; set; }
    public IExpression Left { get; set; }
    public IExpression Right { get; set; }
    public IType Type { get; set; }
}