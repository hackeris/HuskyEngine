using HuskyEngine.Engine.Value;
using HuskyEngine.Tests.Lib;

namespace HuskyEngine.Tests.Function;

[TestClass]
public class SumTTest
{
    [TestMethod]
    public void SumTOnNumberVectorSeriesTest()
    {
        const string formula = "sum_t(earning, -3, 0)";

        var value = (Vector)TestRuntime.Eval(formula);

        Assert.IsTrue(Math.Abs(value.AsNumber()["000001"] - -2.0) < 0.0001f);
        Assert.IsTrue(Math.Abs(value.AsNumber()["000002"] - 2.0) < 0.0001f);
    }
}