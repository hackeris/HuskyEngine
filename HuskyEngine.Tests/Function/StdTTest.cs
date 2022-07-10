using HuskyEngine.Engine.Value;
using HuskyEngine.Tests.Lib;

namespace HuskyEngine.Tests.Function;

[TestClass]
public class StdTTest
{
    [TestMethod]
    public void StdTOnNumberVectorSeriesTest()
    {
        const string formula = "std_t(earning, -3, 0)";

        var value = (Vector)TestRuntime.Eval(formula);

        Assert.IsTrue(value.AsNumber().Values.All(v => Math.Abs(v - 1.118f) < 0.001f));
    }
}