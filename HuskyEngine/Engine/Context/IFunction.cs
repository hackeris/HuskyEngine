using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Types;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Context;

public interface IFunction
{
    public IValue Call(IRuntime runtime, List<IExpression> arguments);
    public IType Type { get; }
    public List<IType> ArgTypes { get; }

    public class Id
    {
        public Id(string name, List<IType> argTypes)
        {
            Name = name;
            ArgTypes = argTypes;
        }

        private bool Equals(Id other)
        {
            return Name == other.Name && ArgTypes.SequenceEqual(other.ArgTypes);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Id)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name,
                ArgTypes.Aggregate(0, (a, b) => a.GetHashCode() ^ b.GetHashCode()));
        }

        public string Name { get; init; }
        public List<IType> ArgTypes { get; init; }
    }

    public class Def
    {
        public string Name { get; set; }
        public IFunction Function { get; set; }
    }
}