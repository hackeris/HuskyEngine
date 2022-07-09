using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Lib.Function.Util;

public static class Mask
{
    public static Vector Apply(Vector vector, Vector mask)
    {
        var masks = mask.AsBoolean();
        return new Vector(vector.ElementType, vector.Values
            .Where(p => masks.GetValueOrDefault(p.Key, false))
            .ToDictionary(p => p.Key, p => p.Value));
    }
}