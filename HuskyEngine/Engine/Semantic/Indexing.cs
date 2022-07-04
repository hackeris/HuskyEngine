using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Semantic;

public class Indexing : IExpression
{
    public IExpression Indexable { get; set; }
    public IExpression Index { get; set; }
    public IType Type { get; set; }
}