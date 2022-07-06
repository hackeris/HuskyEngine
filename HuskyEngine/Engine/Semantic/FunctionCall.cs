using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Semantic;

public class FunctionCall : IExpression
{
    public string Name { get; set; }
    public List<IExpression> Arguments { get; set; }
    public IType Type { get; set; }

    public IFunction.Id GetId()
    {
        return new IFunction.Id
        {
            Name = Name,
            ArgTypes = (
                from argument
                    in Arguments
                select argument.Type
            ).ToList()
        };
    }
}