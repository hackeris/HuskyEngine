using HuskyEngine.Engine.Value;
using HuskyEngine.Tests.Lib;

namespace HuskyEngine.Tests.Function;

[TestClass]
public class CapTest
{
    [TestMethod]
    public void CapIntegerTest()
    {
        const string formula = "cap(earning, 2.0, 4.0)";

        var value = (Vector)TestRuntime.Eval(formula);

        Assert.IsTrue(value.AsNumber().Values.All(v => v is >= 2 and <= 4));
    }
}