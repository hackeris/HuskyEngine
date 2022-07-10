using HuskyEngine.Data;
using HuskyEngine.Engine.Parser;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Context;

public class HuskyRuntime : IRuntime
{
    public HuskyRuntime(IDataSource source)
    {
        _source = source;
        _functions = new Dictionary<IFunction.Id, IFunction>();
    }

    private HuskyRuntime(IDataSource source, Dictionary<IFunction.Id, IFunction> functions)
    {
        _source = source;
        _functions = functions;
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
        return new Predefine(_source, _functions);
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
                Indexable: var expr,
                Index.Type: ScalarType { Type: PrimitiveType.Integer }
            } => Eval(expr, ((Scalar)Eval(indexing.Index)).AsInteger()),
            _ => CallIndex(indexing)
        };
    }

    private IValue CallIndex(Indexing indexing)
    {
        var funcId = new IFunction.Id(
            "[]", new()
            {
                indexing.Indexable.Type,
                indexing.Index.Type
            }
        );
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
        var funcId = new IFunction.Id(
            Operation.NameOf(expression.Operator),
            new List<IType>
            {
                expression.Left.Type,
                expression.Right.Type
            });
        var func = _functions[funcId];
        return func.Call(this, new List<IExpression>
        {
            expression.Left,
            expression.Right
        });
    }

    private IValue Eval(UnaryExpression expression)
    {
        var funcId = new IFunction.Id(
            Operation.NameOf(expression.Operator),
            new List<IType> { expression.Operand.Type }
        );
        var func = _functions[funcId];
        return func.Call(this, new List<IExpression> { expression.Operand });
    }

    private IValue Eval(IExpression expr, int offset)
    {
        if (expr is Identifier identifier)
        {
            var code = identifier.Name;
            if (!_source.IsFormula(code))
            {
                return new Vector(_source.GetVector(code, offset));
            }

            var formula = _source.GetFormula(code);
            var expression = HuskyParser.Parse(formula, GetPredefine());
            return Shift(offset).Eval(expression);
        }

        return Shift(offset).Eval(expr);
    }

    private HuskyRuntime Shift(int offset)
    {
        return offset == 0
            ? this
            : new HuskyRuntime(new OffsetProxySource(_source, offset), _functions);
    }

    public void Register(List<IFunction.Def> defines)
    {
        defines.ForEach(Register);
    }

    private void Register(IFunction.Def define)
    {
        var (name, function) = (define.Name, define.Function);
        var key = new IFunction.Id(name, function.ArgTypes);
        _functions.Add(key, function);
    }

    private readonly IDataSource _source;
    private readonly Dictionary<IFunction.Id, IFunction> _functions;
}