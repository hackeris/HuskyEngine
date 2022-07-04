namespace HuskyEngine.Engine.Semantic;

public class Operation
{
    public enum Binary
    {
        Add,
        Sub,
        Mul,
        Div,

        Power,

        Equal,
        NotEqual,
        Greater,
        Lower,
        GreaterOrEq,
        LowerOrEq
    }

    public enum Unary
    {
        Minus,
        Not,
    }
}