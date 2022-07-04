using HuskyEngine.Engine.Semantic;

namespace HuskyEngine.Engine.Types;

public interface IPredefine
{
    public IType GetType(string symbol);
    public IType GetIndexingType(IType indexable, IType index);

    public IType GetBinaryType(IType left, Operation.Binary op, IType right);

    public IType GetUnaryType(Operation.Unary op, IType operand);
}