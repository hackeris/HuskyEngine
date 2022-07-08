using HuskyEngine.Data.Source;
using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Lib;
using HuskyEngine.Engine.Parser;
using HuskyEngine.Engine.Semantic;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Tests;

[TestClass]
public class FormulaEvaluateTest
{
    [TestMethod]
    public void TestEvaluateFormula()
    {
        const string formula = "earning / price";

        var dataSource = new DataSource(DateTime.Now);
        var runtime = new HuskyRuntime(dataSource);

        BinaryFunction.GetDefines()
            .ForEach(func =>
                runtime.Register(func.Name, func.Function));

        var pred = runtime.GetPredefine();
        IExpression expression = HuskyParser.Parse(formula, pred);

        IValue value = runtime.Eval(expression);

        Console.WriteLine(value);
    }
}