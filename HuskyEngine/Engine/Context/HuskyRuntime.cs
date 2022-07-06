using HuskyEngine.Engine.Parser;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Source;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Context;

public class HuskyRuntime : IRuntime
{
    public HuskyRuntime(DataSource source, DateTime date)
    {
        _source = source;
        _date = date;
        _functions = new Dictionary<IFunction.Id, IFunction>();
    }

    public IValue Eval(IExpression expression)
    {
        return expression switch
        {
            FunctionCall functionCall => Eval(functionCall),
            BinaryExpression binary => Eval(binary),
            UnaryExpression unary => Eval(unary),
            Identifier ident => Eval(ident),
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

    private IValue Eval(Identifier identifier)
    {
        var code = identifier.Name;
        if (_source.IsFormula(code))
        {
            var formula = _source.GetFormula(code);
            return Eval(HuskyParser.Parse(formula, GetPredefine()));
        }
        else
        {
            return _source.GetVector(code, _date, 0);
        }
    }

    private readonly DateTime _date;
    private readonly DataSource _source;
    private readonly Dictionary<IFunction.Id, IFunction> _functions;
}