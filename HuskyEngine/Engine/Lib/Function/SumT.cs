using System.Diagnostics;
using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Function;

public class SumT : IFunction
{
    private SumT(PrimitiveType valueType)
    {
        Type = new VectorType(valueType);
        ArgTypes = new List<IType>
        {
            Type,
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

        var indexes = Enumerable.Range(left, right - left + 1);
        var expr = indexes
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
        return runtime.Eval(expr);
    }

    public IType Type { get; }
    public List<IType> ArgTypes { get; }

    public static List<IFunction.Def> GetDefines()
    {
        var functions = new List<SumT>
        {
            new(PrimitiveType.Integer),
            new(PrimitiveType.Number),
        };
        return functions
            .Select(f => new IFunction.Def { Name = "sum_t", Function = f })
            .ToList();
    }
}