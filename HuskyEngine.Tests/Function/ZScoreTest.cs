using HuskyEngine.Engine.Value;
using HuskyEngine.Tests.Lib;

namespace HuskyEngine.Tests.Function;

[TestClass]
public class ZScoreTest
{
    [TestMethod]
    public void ZScoreOnSameValueVectorTest()
    {
        const string formula = "zscore(earning / price)";

        var value = (Vector)TestRuntime.Eval(formula);

        Assert.IsTrue(value.AsNumber().Values.All(e => e == 0.0f));
    }

    [TestMethod]
    public void ZScoreOnRegularVectorTest()
    {
        const string formula = "zscore(earning)";

        var value = (Vector)TestRuntime.Eval(formula);

        Assert.IsTrue(value.AsNumber()["600000"] == 0.0f);
        Assert.AreEqual(4,
            value.AsNumber().Count(p => p.Value != 0.0f));
    }

    [TestMethod]
    public void ZScoreOnRegularVectorWithScopeTest()
    {
        const string formula = "zscore(earning, earning <= 3)";

        var value = (Vector)TestRuntime.Eval(formula);

        Assert.IsTrue(value.AsNumber()["000002"] == 0.0f);
        Assert.AreEqual(2,
            value.AsNumber().Count(p => p.Value != 0.0f));
    }
}