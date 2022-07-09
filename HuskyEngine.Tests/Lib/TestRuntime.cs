using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Lib;

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
}