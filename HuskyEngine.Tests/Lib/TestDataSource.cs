using HuskyEngine.Data;

namespace HuskyEngine.Tests.Lib;

public class TestDataSource : IDataSource
{
    public bool Exist(string code)
    {
        return code is not ("sum" or "avail");
    }

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

    public Dictionary<string, float> GetVector(string code, int offset)
    {
        if (code == "zero")
        {
            return new Dictionary<string, float>
            {
                { "000001", 0.0f },
                { "000002", 0.0f },
                { "600000", 0.0f },
                { "600001", 0.0f },
                { "688001", 0.0f },
                { "688002", 0.0f },
                { "688003", 0.0f },
            };
        }

        return new Dictionary<string, float>
        {
            { "000001", 1.0f + offset },
            { "000002", 2.0f + offset },
            { "600000", 3.0f + offset },
            { "600001", 4.0f + offset },
            { "600002", 5.0f + offset }
        };
    }
}