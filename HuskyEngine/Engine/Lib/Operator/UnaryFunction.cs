using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Operator;

public class UnaryFunction : IFunction
{
    private UnaryFunction(Operation.Unary op, IType type)
    {
        _op = op;
        Type = type;
    }

    public IValue Call(IRuntime runtime, List<IExpression> arguments)
    {
        var operand = runtime.Eval(arguments[0]);
        return _op switch
        {
            Operation.Unary.Not => Not(operand),
            Operation.Unary.Minus => Minus(operand),
            _ => throw new Exception("unsupported")
        };
    }

    private static IValue Minus(IValue operand)
    {
        return operand switch
        {
            Scalar { ValueType: PrimitiveType.Integer } s =>
                new Scalar(-(int)s.Value),
            Scalar { ValueType: PrimitiveType.Number } s =>
                new Scalar(-(float)s.Value),
            Vector { ElementType: PrimitiveType.Integer } v =>
                new Vector(PrimitiveType.Integer,
                    v.Values.ToDictionary(
                        p => p.Key,
                        p => (object)-(int)p.Value)
                ),
            Vector { ElementType: PrimitiveType.Number } v =>
                new Vector(PrimitiveType.Number,
                    v.Values.ToDictionary(
                        p => p.Key,
                        p => (object)-(float)p.Value)
                ),
            _ => throw new Exception("unsupported")
        };
    }

    private static IValue Not(IValue operand)
    {
        return operand switch
        {
            Scalar { ValueType: PrimitiveType.Boolean } s => ScalarNot(s),
            Vector { ElementType: PrimitiveType.Boolean } v => VectorNot(v),
            _ => throw new Exception("unsupported")
        };
    }

    private static Vector VectorNot(Vector operand)
    {
        return new Vector(PrimitiveType.Boolean,
            operand.Values.ToDictionary(
                pair => pair.Key,
                pair => (object)!(bool)pair.Value)
        );
    }

    private static Scalar ScalarNot(Scalar operand)
    {
        return new Scalar(!(bool)operand.Value);
    }

    public static List<IFunction.Def> GetDefines()
    {
        List<IType> numericTypes = new()
        {
            new ScalarType(PrimitiveType.Integer),
            new VectorType(PrimitiveType.Integer),
            new ScalarType(PrimitiveType.Number),
            new VectorType(PrimitiveType.Number)
        };

        var minusDefines =
            from type in numericTypes
            select new IFunction.Def
            {
                Name = Operation.NameOf(Operation.Unary.Minus),
                Function = new UnaryFunction(Operation.Unary.Minus, type)
            };

        List<IType> logicalTypes = new()
        {
            new ScalarType(PrimitiveType.Boolean),
            new VectorType(PrimitiveType.Boolean)
        };

        var notDefines =
            from type in logicalTypes
            select new IFunction.Def
            {
                Name = Operation.NameOf(Operation.Unary.Not),
                Function = new UnaryFunction(Operation.Unary.Not, type)
            };

        return minusDefines.Union(notDefines).ToList();
    }

    private readonly Operation.Unary _op;

    public IType Type { get; }
    public List<IType> ArgTypes => new() { Type };
}