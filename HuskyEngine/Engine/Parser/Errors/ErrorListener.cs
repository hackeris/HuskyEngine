using Antlr4.Runtime;

namespace HuskyEngine.Engine.Parser.Errors;

public class ErrorListener : BaseErrorListener
{
    public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line,
        int charPositionInLine,
        string msg, RecognitionException e)
    {
        Errors.Add(new ParsingError(line, charPositionInLine, msg));
    }

    public bool HasError()
    {
        return Errors.Count > 0;
    }

    public List<ParsingError> Errors { get; } = new();
}