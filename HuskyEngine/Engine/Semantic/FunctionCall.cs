using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Semantic;

public class FunctionCall : IExpression
{
    public FunctionCall(string name, List<IExpression> arguments, IType type)
    {
        Name = name;
        Arguments = arguments;
        Type = type;
    }

    private string Name { get; }
    public List<IExpression> Arguments { get; }
    public IType Type { get; }

    public IFunction.Id GetId()
    {
        var argTypes = (
            from argument
                in Arguments
            select argument.Type
        ).ToList();
        return new IFunction.Id(Name, argTypes);
    }
}