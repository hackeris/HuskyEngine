using HuskyEngine.Engine.Runtime;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Context;

public class RuntimeProxy : IRuntime
{
    private readonly IRuntime _proxied;
    private readonly int _offset;

    public RuntimeProxy(IRuntime proxied, int offset)
    {
        _proxied = proxied;
        _offset = offset;
    }

    public IValue Eval(IExpression expression)
    {
        return expression switch
        {
            Indexing
                {
                    Indexable: Identifier ident,
                    Index: Literal
                    {
                        Value: int index,
                        Type: ScalarType
                        {
                            Type: PrimitiveType.Integer
                        }
                    }
                } =>
                _proxied.Eval(new Indexing
                {
                    Indexable = ident,
                    Index = new Literal(index + _offset)
                }),
            _ => _proxied.Eval(expression)
        };
    }
}