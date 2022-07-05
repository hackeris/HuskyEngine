using Antlr4.Runtime;
using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Parser;
using HuskyEngine.Engine.Semantic;
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

        var pred = new Predefine();

        var integerScalar = new ScalarType(PrimitiveType.Integer);

        pred.Register("a", integerScalar);
        pred.Register("b", integerScalar);
        pred.Register("sum", new FunctionType(new List<IType>
        {
            integerScalar,
            integerScalar
        }, integerScalar));

        var visitor = new HuskyParser(pred);
        var exp = visitor.Visit(tree);

        Assert.IsTrue(exp is FunctionCall);

        var fc = (FunctionCall)exp;
        Assert.IsTrue(fc.Arguments[0].Type == integerScalar);
        Assert.IsTrue(fc.Arguments[1].Type == integerScalar);
        Assert.IsTrue(fc.Type == integerScalar);
    }
}