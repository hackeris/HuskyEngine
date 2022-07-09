using Antlr4.Runtime;

namespace HuskyEngine.Engine.Parser.Errors;

public class ParsingError : Exception
{
    public ParsingError(ParserRuleContext context, string message) : base(message)
    {
        var line = context.Start.Line;
        var column = context.Start.Column;

        Line = line;
        Column = column + 1;
    }

    public ParsingError(IToken token, string message) : base(message)
    {
        var line = token.Line;
        var column = token.Column;

        Line = line;
        Column = column + 1;
    }

    public override string ToString()
    {
        return $"{Line}:{Column} {Message}";
    }

    public int Line { get; }
    public int Column { get; }
}