using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Semantic;

public class FunctionCall : IExpression
{
    public string Name { get; set; }
    public List<IExpression> Arguments { get; set; }
    public IType Type { get; set; }
}