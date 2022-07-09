using HuskyEngine.Data;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Context;

public class Predefine : IPredefine
{
    public Predefine(
        IDataSource dataSource,
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
        var key = new IFunction.Id(symbol, arguments);

        return _functions.TryGetValue(key, out var found)
            ? new FunctionType(found.ArgTypes, found.Type)
            : null;
    }

    public IType? GetIndexingType(IType left, IType index)
    {
        return index switch
        {
            ScalarType { Type: PrimitiveType.Integer } => left,
            _ => GetFunctionType("[]", new List<IType> { left, index })
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

    private readonly IDataSource _dataDataSource;
    private readonly Dictionary<IFunction.Id, IFunction> _functions;
}