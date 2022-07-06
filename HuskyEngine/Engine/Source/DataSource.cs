using HuskyEngine.Engine.Value;

namespace HuskyEngine.Engine.Source;

public class DataSource
{
    public bool IsFormula(string code)
    {
        if (code == "pe")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetFormula(string code)
    {
        return "price / earning";
    }

    public FloatVector GetVector(string code, DateTime date, int offset)
    {
        return new FloatVector
        {
            Values =
            {
                { "000001", 1.0f },
                { "000002", 2.0f },
                { "600000", 3.0f },
                { "600001", 4.0f }
            }
        };
    }
}