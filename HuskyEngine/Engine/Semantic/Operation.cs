namespace HuskyEngine.Engine.Semantic;

public static class Operation
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

    public static string NameOf(Binary op)
    {
        return op switch
        {
            Binary.Add => "+",
            Binary.Sub => "-",
            Binary.Mul => "*",
            Binary.Div => "/",
            Binary.Power => "^",
            Binary.Equal => "=",
            Binary.NotEqual => "!=",
            Binary.Greater => ">",
            Binary.Lower => "<",
            Binary.GreaterOrEq => ">=",
            Binary.LowerOrEq => "<=",
            _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
        };
    }

    public static string NameOf(Unary op)
    {
        return op switch
        {
            Unary.Minus => "-",
            Unary.Not => "!",
            _ => throw new ArgumentOutOfRangeException(nameof(op), op, null)
        };
    }
}