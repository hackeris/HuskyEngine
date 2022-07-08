namespace HuskyEngine.Engine.Types;

public class FunctionType : IType
{
    public FunctionType(List<IType> arguments, IType returnType)
    {
        Arguments = arguments;
        ReturnType = returnType;
    }

    public List<IType> Arguments { get; }
    public IType ReturnType { get; }

    private bool Equals(FunctionType other)
    {
        return Arguments.Equals(other.Arguments) && ReturnType.Equals(other.ReturnType);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((FunctionType)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Arguments, ReturnType);
    }
}