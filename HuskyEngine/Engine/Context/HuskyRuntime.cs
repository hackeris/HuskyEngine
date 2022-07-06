using HuskyEngine.Data.Source;
using HuskyEngine.Engine.Parser;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Context;

public class HuskyRuntime : IRuntime
{
    public HuskyRuntime(DataSource source)
    {
        _source = source;
        _functions = new Dictionary<IFunction.Id, IFunction>();
    }

    public IValue Eval(IExpression expression)
    {
        return expression switch
        {
            FunctionCall functionCall => Eval(functionCall),
            BinaryExpression binary => Eval(binary),
            UnaryExpression unary => Eval(unary),
            Identifier ident => Eval(ident, 0),
            Indexing indexing => Eval(indexing),
            Literal literal => Eval(literal),
            _ => throw new ArgumentOutOfRangeException(nameof(expression), expression, null)
        };
    }

    public IPredefine GetPredefine()
    {
        throw new NotImplementedException();
    }

    private IValue Eval(Literal literal)
    {
        var type = ((ScalarType)literal.Type).Type;
        return type switch
        {
            PrimitiveType.Integer => new Scalar((int)literal.Value),
            PrimitiveType.Number => new Scalar((float)literal.Value),
            PrimitiveType.Boolean => new Scalar((bool)literal.Value),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private IValue Eval(Indexing indexing)
    {
        return indexing switch
        {
            {
                Indexable: var exp,
                Index: Literal
                {
                    Value: int index,
                    Type: ScalarType { Type: PrimitiveType.Integer }
                }
            } => ProxyOffset(exp, index),
            _ => CallIndex(indexing)
        };
    }

    private IValue CallIndex(Indexing indexing)
    {
        var funcId = new IFunction.Id
        {
            Name = "[]",
            ArgTypes =
            {
                indexing.Indexable.Type,
                indexing.Index.Type
            }
        };
        var func = _functions[funcId];
        return func.Call(this, new List<IExpression>
        {
            indexing.Indexable,
            indexing.Index
        });
    }

    private IValue Eval(FunctionCall functionCall)
    {
        var funcId = functionCall.GetId();
        var func = _functions[funcId];
        return func.Call(this, functionCall.Arguments);
    }

    private IValue Eval(BinaryExpression expression)
    {
        var funcId = new IFunction.Id
        {
            Name = Operation.NameOf(expression.Operator),
            ArgTypes =
            {
                expression.Left.Type,
                expression.Right.Type
            }
        };
        var func = _functions[funcId];
        return func.Call(this, new List<IExpression>
        {
            expression.Left,
            expression.Right
        });
    }

    private IValue Eval(UnaryExpression expression)
    {
        var funcId = new IFunction.Id
        {
            Name = Operation.NameOf(expression.Operator),
            ArgTypes = { expression.Operand.Type }
        };
        var func = _functions[funcId];
        return func.Call(this, new List<IExpression> { expression.Operand });
    }

    private IValue Eval(Identifier identifier, int offset)
    {
        var code = identifier.Name;
        if (!_source.IsFormula(code))
        {
            return new FloatVector { Values = _source.GetVector(code, offset) };
        }

        var formula = _source.GetFormula(code);
        var expression = HuskyParser.Parse(formula, GetPredefine());

        return ProxyOffset(expression, offset);
    }

    private IValue ProxyOffset(IExpression expression, int offset)
    {
        IRuntime runtime = offset != 0
            ? new OffsetProxy(this, offset)
            : this;
        return runtime.Eval(expression);
    }

    public void Register(string name, IFunction function)
    {
        var key = new IFunction.Id
        {
            Name = name,
            ArgTypes = function.ArgTypes
        };
        _functions.Add(key, function);
    }

    private readonly DataSource _source;
    private readonly Dictionary<IFunction.Id, IFunction> _functions;
}