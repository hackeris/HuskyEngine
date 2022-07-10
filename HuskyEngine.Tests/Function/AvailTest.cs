using HuskyEngine.Engine.Value;
using HuskyEngine.Tests.Lib;

namespace HuskyEngine.Tests.Function;

[TestClass]
public class AvailTest
{
    [TestMethod]
    public void AvailNumberVectorWithIntegerDefaultValueTest()
    {
        const string formula = "avail(price, 1000)";

        var value = (Vector)TestRuntime.Eval(formula);

        Assert.AreEqual(1000.0f, value.AsNumber()["688003"]);
    }
}