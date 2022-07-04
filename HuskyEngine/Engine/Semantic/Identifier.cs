using HuskyEngine.Engine.Types;

namespace HuskyEngine.Engine.Semantic;

public class Identifier : IExpression
{
    public Identifier(string name, IType type)
    {
        Name = name;
        Type = type;
    }

    public string Name { get; }
    public IType Type { get; }
}