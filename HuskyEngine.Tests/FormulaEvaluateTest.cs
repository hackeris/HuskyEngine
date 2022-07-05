using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Parser;
using HuskyEngine.Engine.Runtime;
using HuskyEngine.Engine.Semantic;

namespace HuskyEngine.Tests;

[TestClass]
public class FormulaEvaluateTest
{
    class ContextFactory
    {
        public IRuntime GetRuntime()
        {
            return null;
        }

        public IPredefine GetPredefine()
        {
            return null;
        }
    }

    [TestMethod]
    public void TestEvaluateFormula()
    {
        string formula = "earning / price";

        ContextFactory context = new ContextFactory();

        var pred = context.GetPredefine();
        IExpression expression = HuskyParser.Parse(formula, pred);

        var runtime = context.GetRuntime();
        IValue value = runtime.Eval(expression);

        Console.WriteLine(value);
    }
}