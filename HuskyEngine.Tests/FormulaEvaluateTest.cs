using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Parser;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Source;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Tests;

[TestClass]
public class FormulaEvaluateTest
{

    [TestMethod]
    public void TestEvaluateFormula()
    {
        string formula = "earning / price";

        var dataSource = new DataSource();
        var runtime = new HuskyRuntime(dataSource, DateTime.Now);

        var pred = runtime.GetPredefine();
        IExpression expression = HuskyParser.Parse(formula, pred);

        IValue value = runtime.Eval(expression);

        Console.WriteLine(value);
    }
}