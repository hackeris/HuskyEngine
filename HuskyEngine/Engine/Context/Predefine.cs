using HuskyEngine.Data.Source;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Context;

public class Predefine : IPredefine
{
    public Predefine(
        DataSource dataSource,
        Dictionary<IFunction.Id, IFunction> functions
    )
    {
        _dataDataSource = dataSource;
        _functions = functions;
    }

    public IType? GetType(string symbol)
    {
        return _dataDataSource.Exist(symbol)
            ? new VectorType(PrimitiveType.Number)
            : null;
    }

    public FunctionType? GetFunctionType(string symbol, List<IType> arguments)
    {
        var key = new IFunction.Id
        {
            Name = symbol,
            ArgTypes = arguments
        };

        return _functions.TryGetValue(key, out var found)
            ? new FunctionType(found.ArgTypes, found.Type)
            : null;
    }

    public IType? GetIndexingType(IType indexable, IType index)
    {
        return index switch
        {
            ScalarType { Type: PrimitiveType.Integer } => indexable,
            _ => GetFunctionType("[]", new List<IType> { indexable, index })
        };
    }

    public IType? GetBinaryType(IType left, Operation.Binary op, IType right)
    {
        return GetFunctionType(Operation.NameOf(op), new List<IType> { left, right });
    }

    public IType? GetUnaryType(Operation.Unary op, IType operand)
    {
        return GetFunctionType(Operation.NameOf(op), new List<IType> { operand });
    }

    private readonly DataSource _dataDataSource;
    private readonly Dictionary<IFunction.Id, IFunction> _functions;
}