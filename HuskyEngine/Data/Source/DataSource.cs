namespace HuskyEngine.Data.Source;

public class DataSource
{
    public DataSource(DateTime baseDate)
    {
        _baseDate = baseDate;
    }

    public bool Exist(string code)
    {
        return true;
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
        return new Dictionary<string, float>
        {
            { "000001", 1.0f },
            { "000002", 2.0f },
            { "600000", 3.0f },
            { "600001", 4.0f }
        };
    }

    private readonly DateTime _baseDate;
}