using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Lib;
using HuskyEngine.Engine.Parser;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Tests.Lib;

public static class TestRuntime
{
    public static IRuntime Get()
    {
        var dataSource = new TestDataSource();
        var runtime = new HuskyRuntime(dataSource);
        runtime.Register(Definition.GetDefines());
        return runtime;
    }

    public static IValue Eval(string formula)
    {
        var runtime = Get();

        var expression = HuskyParser.Parse(formula, runtime);

        return runtime.Eval(expression);
    }
}