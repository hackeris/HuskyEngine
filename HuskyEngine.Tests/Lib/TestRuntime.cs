using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Lib;
using HuskyEngine.Engine.Lib.Operator;

namespace HuskyEngine.Tests.Lib;

public static class TestRuntime
{
    public static IRuntime Get()
    {
        var dataSource = new TestDataSource();
        var runtime = new HuskyRuntime(dataSource);
        runtime.Register(BinaryFunction.GetDefines());
        runtime.Register(UnaryFunction.GetDefines());
        runtime.Register(Sum.GetDefines());
        runtime.Register(Rank.GetDefines());
        runtime.Register(Avail.GetDefines());
        return runtime;
    }
}