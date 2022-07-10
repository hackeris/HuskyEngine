using System.Diagnostics;
using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Lib.Operator;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Function;

public class StdT : IFunction
{
    private StdT(PrimitiveType valueType)
    {
        Type = new VectorType(PrimitiveType.Number);
        ArgTypes = new List<IType>
        {
            new VectorType(valueType),
            new ScalarType(PrimitiveType.Integer),
            new ScalarType(PrimitiveType.Integer)
        };
    }

    public IValue Call(IRuntime runtime, List<IExpression> arguments)
    {
        var seriesExpr = arguments[0];
        var (left, right) = (
            ((Scalar)runtime.Eval(arguments[1])).AsInteger(),
            ((Scalar)runtime.Eval(arguments[2])).AsInteger()
        );

        Debug.Assert(left < right && right <= 0);

        var count = right - left + 1;
        var indexes = Enumerable.Range(left, count);

        var series = indexes
            .Select<int, IExpression>(offset => new Indexing
            {
                Indexable = seriesExpr,
                Index = new Literal(offset),
                Type = seriesExpr.Type
            })
            .Select(runtime.Eval)
            .ToList();

        var sum = series
            .Aggregate((a, b) => BinaryFunction.Apply(a, Operation.Binary.Add, b));

        var avg = BinaryFunction.Apply(sum, Operation.Binary.Div, new Scalar(count));

        var sumSquare = series
            .Select(s => BinaryFunction.Apply(s, Operation.Binary.Sub, avg))
            .Select(s => BinaryFunction.Apply(s, Operation.Binary.Mul, s))
            .Aggregate((a, b) => BinaryFunction.Apply(a, Operation.Binary.Add, b));

        var squareVar = BinaryFunction.Apply(sumSquare, Operation.Binary.Div, new Scalar(count));

        var values = ((Vector)squareVar).AsNumber()
            .ToDictionary(p => p.Key, p => (float)Math.Sqrt(p.Value));
        return new Vector(values);
    }

    public IType Type { get; }
    public List<IType> ArgTypes { get; }

    public static List<IFunction.Def> GetDefines()
    {
        var functions = new List<StdT>
        {
            new(PrimitiveType.Integer),
            new(PrimitiveType.Number),
        };
        return functions
            .Select(f => new IFunction.Def { Name = "std_t", Function = f })
            .ToList();
    }
}