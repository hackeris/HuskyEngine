using HuskyEngine.Engine.Parser;
using HuskyEngine.Engine.Value;
using HuskyEngine.Tests.Lib;

namespace HuskyEngine.Tests;

[TestClass]
public class FormulaEvaluateTest
{
    [TestMethod]
    public void TestEvaluateBinary()
    {
        const string formula = "earning / price";

        var value = (Vector)TestEval(formula);

        Assert.IsTrue(value.AsNumber().All(p => Math.Abs(p.Value - 1.0) < 0.00001f));
    }

    [TestMethod]
    public void TestEvaluateUnary()
    {
        const string formula = "!(- earning / price > 0)";

        var value = (Vector)TestEval(formula);

        Assert.IsTrue(value.AsBoolean().All(p => p.Value));
    }

    [TestMethod]
    public void TestEvaluateLogical()
    {
        const string formula = "earning < 0 | price > 0";

        var value = (Vector)TestEval(formula);

        Assert.IsTrue(value.AsBoolean().All(p => p.Value));
    }

    [TestMethod]
    public void TestEvaluateNested()
    {
        const string formula = "pe + pe = 2";

        var value = (Vector)TestEval(formula);

        Assert.IsTrue(value.AsBoolean().All(p => p.Value));
    }

    [TestMethod]
    public void TestEvaluateIndex()
    {
        const string formula = "pe[-2] + pe[-1]";

        var value = (Vector)TestEval(formula);

        Assert.IsTrue(value.AsNumber().All(p => Math.Abs(p.Value - 2.0) < 0.0001f));
    }

    [TestMethod]
    public void TestEvaluateFunctionCall()
    {
        const string formula = "sum(pe, price > 2)";

        var value = (Scalar)TestEval(formula);

        Assert.IsTrue(Math.Abs(value.AsNumber() - 2.0f) < 0.0001f);
    }

    [TestMethod]
    public void TestEvaluateSum()
    {
        const string formula = "sum(pe)";

        var value = (Scalar)TestEval(formula);

        Assert.IsTrue(Math.Abs(value.AsNumber() - 4.0f) < 0.0001f);
    }

    [TestMethod]
    public void TestRankFunction()
    {
        const string formula = "rank(-price)";

        var value = (Vector)TestEval(formula);

        Assert.AreEqual(4, value.AsInteger()["000001"]);
    }

    [TestMethod]
    public void TestAvailFunction()
    {
        const string formula = "avail(rank(-price), 1000)";

        var value = (Vector)TestEval(formula);

        Assert.AreEqual(1000, value.Values["688001"]);
    }

    private static IValue TestEval(string formula)
    {
        var runtime = TestRuntime.Get();

        var expression = HuskyParser.Parse(formula, runtime);

        return runtime.Eval(expression);
    }
}