using Antlr4.Runtime;
using HuskyEngine.Engine.Evaluator;
using HuskyEngine.Engine.Parser;
using HuskyEngine.Engine.Types;

namespace HuskyEngine.Tests;

[TestClass]
public class ParseExpressionTest
{
    [TestMethod]
    public void TestParseFunctionCall()
    {
        string code = "sum(a, b)";

        var stream = new AntlrInputStream(code);
        var lexer = new HuskyLangLexer(stream);
        var tokens = new CommonTokenStream(lexer);
        var parser = new HuskyLangParser(tokens);
        var tree = parser.statement();

        var pred = new HuskyPredefine();

        pred.Register("a", new ScalarType(PrimitiveType.Integer));
        pred.Register("b", new ScalarType(PrimitiveType.Integer));
        pred.Register("sum", new FuncType(new List<IType>
        {
            new ScalarType(PrimitiveType.Integer),
            new ScalarType(PrimitiveType.Integer)
        }, new ScalarType(PrimitiveType.Integer)));

        var visitor = new HuskyParser(pred);
        var exp = visitor.Visit(tree);

        Console.WriteLine("exp is " + exp);
    }
}