using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Context;

public interface IPredefine
{
    public IType? GetType(string symbol);

    public FunctionType? GetFunctionType(string symbol, List<IType> arguments);

    public IType? GetIndexingType(IType indexable, IType index);

    public IType? GetBinaryType(IType left, Operation.Binary op, IType right);

    public IType? GetUnaryType(Operation.Unary op, IType operand);
}