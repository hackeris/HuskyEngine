using HuskyEngine.Engine.Parser;
using HuskyEngine.Engine.Parser.Errors;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Tests.Lib;

namespace HuskyEngine.Tests;

[TestClass]
public class FormulaParsingErrorTest
{
    [TestMethod]
    public void BinaryOperatorNotSupportedTest()
    {
        const string code = "1.0 + (1>0)";

        try
        {
            TestParse(code);
            Assert.Fail();
        }
        catch (ParsingError e)
        {
            Assert.IsTrue(e.Message.Contains("Unsupported binary operator + on number and bool"));
        }
    }

    [TestMethod]
    public void FunctionNotFoundTest()
    {
        const string code = "1.0 + func(2.0, 5, price)";

        try
        {
            TestParse(code);
            Assert.Fail();
        }
        catch (ParsingError e)
        {
            Assert.IsTrue(e.Message.Contains("Could not find function func"));
        }
    }

    [TestMethod]
    public void IdentifierNotFoundTest()
    {
        const string code = "1.0 + sum[2]";

        try
        {
            TestParse(code);
            Assert.Fail();
        }
        catch (ParsingError e)
        {
            Assert.IsTrue(e.Message.Contains("Identifier sum is not found"));
        }
    }

    [TestMethod]
    public void UnaryOperatorNotSupportedTest()
    {
        const string code = "-(1 > 0)";

        try
        {
            TestParse(code);
            Assert.Fail();
        }
        catch (ParsingError e)
        {
            Assert.IsTrue(e.Message.Contains("Unsupported operator - on bool"));
        }
    }

    [TestMethod]
    public void FunctionCallParsingTest()
    {
        const string code = "sum(price)";

        var exp = TestParse(code);
        Assert.IsTrue(exp is FunctionCall);
    }

    private static IExpression TestParse(string formula)
    {
        var runtime = TestRuntime.Get();
        return HuskyParser.Parse(formula, runtime);
    }
}