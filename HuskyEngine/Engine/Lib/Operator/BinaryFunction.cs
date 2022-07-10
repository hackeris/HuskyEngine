using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Operator;

public class BinaryFunction : IFunction
{
    private BinaryFunction(Operation.Binary op, IType left, IType right)
    {
        _op = op;
        Type = RouteType(left, op, right);
        ArgTypes = new List<IType> { left, right };
    }

    private IType RouteType(IType left, Operation.Binary op, IType right)
    {
        return (left, right) switch
        {
            (VectorType l, VectorType r) => VectorOnly(l, op, r),
            (ScalarType l, VectorType r) => ScalarVector(l, op, r),
            (VectorType l, ScalarType r) => VectorScalar(l, op, r),
            (ScalarType l, ScalarType r) => ScalarOnly(l, op, r),
            _ => throw new Exception("unsupported")
        };
    }

    public IValue Call(IRuntime runtime, List<IExpression> arguments)
    {
        var left = runtime.Eval(arguments[0]);
        var right = runtime.Eval(arguments[1]);

        return Apply(left, _op, right);
    }

    public static IValue Apply(IValue left, Operation.Binary op, IValue right)
    {
        return (left, right) switch
        {
            (Vector l, Vector r) => VectorOnly(l, op, r),
            (Vector l, Scalar r) => VectorScalar(l, op, r),
            (Scalar l, Vector r) => ScalarVector(l, op, r),
            (Scalar l, Scalar r) => ScalarOnly(l, op, r),
            _ => throw new Exception("unsupported")
        };
    }

    private static IValue VectorOnly(Vector left, Operation.Binary op, Vector right)
    {
        var leftValues = left.Values;
        var rightValues = right.Values;

        var keys = leftValues.Keys.Union(rightValues.Keys);

        var func = SelectFunction(left.ElementType, op, right.ElementType);
        var values = keys
            .Where(key =>
                leftValues.ContainsKey(key))
            .Where(key =>
                rightValues.ContainsKey(key))
            .ToDictionary(key => key, key =>
                func(leftValues[key], rightValues[key]))
            .Where(pair => pair.Value != null)
            .ToDictionary(pair => pair.Key, pair => pair.Value!);

        return new Vector(PrimitiveOnly(left.ElementType, op, right.ElementType), values);
    }

    private static IValue VectorScalar(Vector left, Operation.Binary op, Scalar right)
    {
        return VectorOnly(left, op, new Vector(left.Values.Keys, right));
    }

    private static IValue ScalarVector(Scalar left, Operation.Binary op, Vector right)
    {
        return VectorOnly(new Vector(right.Values.Keys, left), op, right);
    }

    private static Scalar ScalarOnly(Scalar left, Operation.Binary op, Scalar right)
    {
        var func = SelectFunction(left.ValueType, op, right.ValueType);
        return new Scalar(
            PrimitiveOnly(left.ValueType, op, right.ValueType),
            func(left.Value, right.Value)!
        );
    }

    private static Func<object, object, object?> SelectFunction(PrimitiveType left, Operation.Binary op,
        PrimitiveType right)
    {
        return (left, right) switch
        {
            (PrimitiveType.Integer, PrimitiveType.Integer) => IntegerOps(op),
            (PrimitiveType.Number, PrimitiveType.Number) => NumberOps(op),
            (PrimitiveType.Integer, PrimitiveType.Number) => IntegerNumberOps(op),
            (PrimitiveType.Number, PrimitiveType.Integer) => NumberIntegerOps(op),
            (PrimitiveType.Boolean, PrimitiveType.Boolean) => BooleanOps(op),
            _ => throw new Exception("unsupported")
        };
    }

    private static Func<object, object, object?> BooleanOps(Operation.Binary op)
    {
        return op switch
        {
            Operation.Binary.And =>
                (a, b) => (bool)a && (bool)b,
            Operation.Binary.Or =>
                (a, b) => (bool)a || (bool)b,
            _ => throw new Exception("unsupported")
        };
    }

    private static Func<object, object, object?> IntegerOps(Operation.Binary op)
    {
        return op switch
        {
            Operation.Binary.Add =>
                (a, b) => (int)a + (int)b,
            Operation.Binary.Sub =>
                (a, b) => (int)a - (int)b,
            Operation.Binary.Mul =>
                (a, b) => (int)a * (int)b,
            Operation.Binary.Div =>
                (a, b) => (int)b != 0 ? (int)a / (int)b : null,
            Operation.Binary.Power =>
                (a, b) => (int)Math.Pow((int)a, (int)b),
            Operation.Binary.Equal =>
                (a, b) => (int)a == (int)b,
            Operation.Binary.NotEqual =>
                (a, b) => (int)a != (int)b,
            Operation.Binary.Greater =>
                (a, b) => (int)a > (int)b,
            Operation.Binary.Lower =>
                (a, b) => (int)a < (int)b,
            Operation.Binary.GreaterOrEq =>
                (a, b) => (int)a >= (int)b,
            Operation.Binary.LowerOrEq =>
                (a, b) => (int)a <= (int)b,
            _ => throw new Exception("unsupported")
        };
    }

    private static Func<object, object, object?> NumberOps(Operation.Binary op)
    {
        return op switch
        {
            Operation.Binary.Add =>
                (a, b) => (float)a + (float)b,
            Operation.Binary.Sub =>
                (a, b) => (float)a - (float)b,
            Operation.Binary.Mul =>
                (a, b) => (float)a * (float)b,
            Operation.Binary.Div =>
                (a, b) => (float)b != 0.0 ? (float)a / (float)b : null,
            Operation.Binary.Power =>
                (a, b) => (float)Math.Pow((float)a, (float)b),
            Operation.Binary.Equal =>
                (a, b) => Math.Abs((float)a - (float)b) < 0.00001f,
            Operation.Binary.NotEqual =>
                (a, b) => Math.Abs((float)a - (float)b) > 0.00001f,
            Operation.Binary.Greater =>
                (a, b) => (float)a > (float)b,
            Operation.Binary.Lower =>
                (a, b) => (float)a < (float)b,
            Operation.Binary.GreaterOrEq =>
                (a, b) => (float)a >= (float)b,
            Operation.Binary.LowerOrEq =>
                (a, b) => (float)a <= (float)b,
            _ => throw new Exception("unsupported")
        };
    }

    private static Func<object, object, object?> NumberIntegerOps(Operation.Binary op)
    {
        return op switch
        {
            Operation.Binary.Add =>
                (a, b) => (float)a + (int)b,
            Operation.Binary.Sub =>
                (a, b) => (float)a - (int)b,
            Operation.Binary.Mul =>
                (a, b) => (float)a * (int)b,
            Operation.Binary.Div =>
                (a, b) => (int)b != 0 ? (float)a / (int)b : null,
            Operation.Binary.Power =>
                (a, b) => (float)Math.Pow((float)a, (int)b),
            Operation.Binary.Equal =>
                (a, b) => Math.Abs((float)a - (int)b) < 0.00001f,
            Operation.Binary.NotEqual =>
                (a, b) => Math.Abs((float)a - (int)b) > 0.00001f,
            Operation.Binary.Greater =>
                (a, b) => (float)a > (int)b,
            Operation.Binary.Lower =>
                (a, b) => (float)a < (int)b,
            Operation.Binary.GreaterOrEq =>
                (a, b) => (float)a >= (int)b,
            Operation.Binary.LowerOrEq =>
                (a, b) => (float)a <= (int)b,
            _ => throw new Exception("unsupported")
        };
    }

    private static Func<object, object, object?> IntegerNumberOps(Operation.Binary op)
    {
        return op switch
        {
            Operation.Binary.Add =>
                (a, b) => (int)a + (float)b,
            Operation.Binary.Sub =>
                (a, b) => (int)a - (float)b,
            Operation.Binary.Mul =>
                (a, b) => (int)a * (float)b,
            Operation.Binary.Div =>
                (a, b) => (float)b != 0 ? (int)a / (float)b : null,
            Operation.Binary.Power =>
                (a, b) => (float)Math.Pow((int)a, (float)b),
            Operation.Binary.Equal =>
                (a, b) => Math.Abs((int)a - (float)b) < 0.00001f,
            Operation.Binary.NotEqual =>
                (a, b) => Math.Abs((int)a - (float)b) > 0.00001f,
            Operation.Binary.Greater =>
                (a, b) => (int)a > (float)b,
            Operation.Binary.Lower =>
                (a, b) => (int)a < (float)b,
            Operation.Binary.GreaterOrEq =>
                (a, b) => (int)a >= (float)b,
            Operation.Binary.LowerOrEq =>
                (a, b) => (int)a <= (float)b,
            _ => throw new Exception("unsupported")
        };
    }

    private static IType VectorOnly(VectorType left, Operation.Binary op, VectorType right)
    {
        return new VectorType(PrimitiveOnly(left.ElementType, op, right.ElementType));
    }

    private static IType VectorScalar(VectorType left, Operation.Binary op, ScalarType right)
    {
        return new VectorType(PrimitiveOnly(left.ElementType, op, right.Type));
    }

    private static IType ScalarVector(ScalarType left, Operation.Binary op, VectorType right)
    {
        return new VectorType(PrimitiveOnly(left.Type, op, right.ElementType));
    }

    private static IType ScalarOnly(ScalarType left, Operation.Binary op, ScalarType right)
    {
        return new ScalarType(PrimitiveOnly(left.Type, op, right.Type));
    }

    private static PrimitiveType PrimitiveOnly(PrimitiveType left, Operation.Binary op, PrimitiveType right)
    {
        return (left, right) switch
        {
            (PrimitiveType.Integer, PrimitiveType.Integer) => op switch
            {
                Operation.Binary.Add
                    or Operation.Binary.Sub
                    or Operation.Binary.Mul
                    or Operation.Binary.Div
                    or Operation.Binary.Power
                    => PrimitiveType.Integer,
                Operation.Binary.Equal
                    or Operation.Binary.NotEqual
                    or Operation.Binary.Greater
                    or Operation.Binary.Lower
                    or Operation.Binary.GreaterOrEq
                    or Operation.Binary.LowerOrEq
                    => PrimitiveType.Boolean,
                _ => throw new Exception("unsupported")
            },
            (PrimitiveType.Number, PrimitiveType.Integer)
                or (PrimitiveType.Integer, PrimitiveType.Number)
                or (PrimitiveType.Number, PrimitiveType.Number)
                => op switch
                {
                    Operation.Binary.Add
                        or Operation.Binary.Sub
                        or Operation.Binary.Mul
                        or Operation.Binary.Div
                        or Operation.Binary.Power
                        => PrimitiveType.Number,
                    Operation.Binary.Equal
                        or Operation.Binary.NotEqual
                        or Operation.Binary.Greater
                        or Operation.Binary.Lower
                        or Operation.Binary.GreaterOrEq
                        or Operation.Binary.LowerOrEq
                        => PrimitiveType.Boolean,
                    _ => throw new Exception("unsupported")
                },
            (PrimitiveType.Boolean, PrimitiveType.Boolean) => op switch
            {
                Operation.Binary.And or Operation.Binary.Or => PrimitiveType.Boolean,
                _ => throw new Exception("unsupported")
            },
            _ => throw new Exception("unsupported")
        };
    }

    private readonly Operation.Binary _op;

    public IType Type { get; }

    public List<IType> ArgTypes { get; }

    public static List<IFunction.Def> GetDefines()
    {
        List<Operation.Binary> numericOperators = new()
        {
            Operation.Binary.Add,
            Operation.Binary.Sub,
            Operation.Binary.Mul,
            Operation.Binary.Div,
            Operation.Binary.Power,
            Operation.Binary.Equal,
            Operation.Binary.NotEqual,
            Operation.Binary.Greater,
            Operation.Binary.Lower,
            Operation.Binary.GreaterOrEq,
            Operation.Binary.LowerOrEq,
        };
        List<IType> numericTypes = new()
        {
            new ScalarType(PrimitiveType.Integer),
            new VectorType(PrimitiveType.Integer),
            new ScalarType(PrimitiveType.Number),
            new VectorType(PrimitiveType.Number)
        };

        List<Operation.Binary> logicalOperators = new()
        {
            Operation.Binary.And,
            Operation.Binary.Or
        };
        List<IType> logicalTypes = new()
        {
            new ScalarType(PrimitiveType.Boolean),
            new VectorType(PrimitiveType.Boolean)
        };

        var numericDefines =
            from op in numericOperators
            from left in numericTypes
            from right in numericTypes
            select new IFunction.Def
            {
                Name = Operation.NameOf(op),
                Function = new BinaryFunction(op, left, right)
            };

        var logicalDefines =
            from op in logicalOperators
            from left in logicalTypes
            from right in logicalTypes
            select new IFunction.Def
            {
                Name = Operation.NameOf(op),
                Function = new BinaryFunction(op, left, right)
            };

        return numericDefines.Union(logicalDefines).ToList();
    }
}