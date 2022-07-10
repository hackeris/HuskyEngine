using System.Diagnostics;
using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Function;

public class AvgT : IFunction
{
    private AvgT(PrimitiveType valueType)
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
        var series = arguments[0];
        var (left, right) = (
            ((Scalar)runtime.Eval(arguments[1])).AsInteger(),
            ((Scalar)runtime.Eval(arguments[2])).AsInteger()
        );

        Debug.Assert(left < right && right <= 0);

        var count = right - left + 1;
        var indexes = Enumerable.Range(left, count);
        var sumExpr = indexes
            .Select<int, IExpression>(offset => new Indexing
            {
                Indexable = series,
                Index = new Literal(offset),
                Type = series.Type
            })
            .Aggregate((a, b) => new BinaryExpression
            {
                Left = a,
                Right = b,
                Operator = Operation.Binary.Add,
                Type = Type
            });
        var avgExpr = new BinaryExpression
        {
            Left = sumExpr,
            Right = new Literal(count),
            Operator = Operation.Binary.Div,
            Type = sumExpr.Type
        };
        return runtime.Eval(avgExpr);
    }

    public IType Type { get; }
    public List<IType> ArgTypes { get; }

    public static List<IFunction.Def> GetDefines()
    {
        var functions = new List<AvgT>
        {
            new(PrimitiveType.Integer),
            new(PrimitiveType.Number),
        };
        return functions
            .Select(f => new IFunction.Def { Name = "avg_t", Function = f })
            .ToList();
    }
}