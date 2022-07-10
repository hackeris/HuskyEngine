using HuskyEngine.Engine.Context;
using HuskyEngine.Engine.Lib.Function;
using HuskyEngine.Engine.Lib.Operator;

namespace HuskyEngine.Engine.Lib;

public static class Definition
{
    public static List<IFunction.Def> GetDefines()
    {
        return new List<List<IFunction.Def>>
        {
            BinaryFunction.GetDefines(),
            UnaryFunction.GetDefines(),
            Sum.GetDefines(),
            Rank.GetDefines(),
            Avail.GetDefines(),
            Avg.GetDefines(),
            Std.GetDefines(),
            ZScore.GetDefines()
        }.Aggregate((a, b) => a.Union(b).ToList());
    }
}