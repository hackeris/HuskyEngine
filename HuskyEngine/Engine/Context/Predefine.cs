using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Context;

public class Predefine : IPredefine
{
    public Predefine()
    {
        _types = new Dictionary<string, IType>();
    }

    public IType? GetType(string symbol)
    {
        return _types[symbol];
    }

    public FunctionType? GetFunctionType(string symbol, List<IType> arguments)
    {
        return (FunctionType)_types[symbol];
    }

    public IType? GetIndexingType(IType indexable, IType index)
    {
        return null;
    }

    public IType? GetBinaryType(IType left, Operation.Binary op, IType right)
    {
        return null;
    }

    public IType? GetUnaryType(Operation.Unary op, IType operand)
    {
        return null;
    }

    public void Register(string symbol, IType type)
    {
        _types.Add(symbol, type);
    }

    private readonly Dictionary<string, IType> _types;
}