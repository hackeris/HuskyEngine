namespace HuskyEngine.Data;

public interface IDataSource
{
    public bool Exist(string code);

    public bool IsFormula(string code);

    public string GetFormula(string code);

    public Dictionary<string, float> GetVector(string code, int offset);
}