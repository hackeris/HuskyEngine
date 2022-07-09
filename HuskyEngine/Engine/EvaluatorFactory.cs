using HuskyEngine.Data;
using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Lib;
using HuskyEngine.Engine.Parser;
using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine;

public class EvaluatorFactory
{
    public EvaluatorFactory(DataSourceFactory source)
    {
        _source = source;
    }

    public Evaluator At(DateTime date)
    {
        var dataSource = _source.At(date);
        var runtime = new HuskyRuntime(dataSource);
        runtime.Register(Definition.GetDefines());
        return new Evaluator(runtime);
    }

    private readonly DataSourceFactory _source;

    public class Evaluator
    {
        public Evaluator(IRuntime runtime)
        {
            _runtime = runtime;
        }

        public IValue Eval(string code)
        {
            var expr = HuskyParser.Parse(code, _runtime.GetPredefine());
            return _runtime.Eval(expr);
        }

        private readonly IRuntime _runtime;
    }
}