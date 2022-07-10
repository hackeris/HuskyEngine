using HuskyEngine.Engine.Value;
using HuskyEngine.Tests.Lib;

namespace HuskyEngine.Tests.Function;

[TestClass]
public class AvgTTest
{
    [TestMethod]
    public void AvgTOnNumberVectorSeriesTest()
    {
        const string formula = "avg_t(earning, -3, 0)";

        var value = (Vector)TestRuntime.Eval(formula);

        Assert.IsTrue(Math.Abs(value.AsNumber()["000001"] - -0.5) < 0.0001f);
        Assert.IsTrue(Math.Abs(value.AsNumber()["000002"] - 0.5) < 0.0001f);
    }
}