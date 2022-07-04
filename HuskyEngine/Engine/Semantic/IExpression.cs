using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Semantic;

public interface IExpression
{
    IType Type { get; }
}