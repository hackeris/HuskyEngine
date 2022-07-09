namespace HuskyEngine.Data;

public class OffsetProxySource : IDataSource
{
    public OffsetProxySource(IDataSource proxied, int offset)
    {
        _proxied = proxied;
        _offset = offset;
    }

    public bool Exist(string code)
    {
        return _proxied.Exist(code);
    }

    public bool IsFormula(string code)
    {
        return _proxied.IsFormula(code);
    }

    public string GetFormula(string code)
    {
        return _proxied.GetFormula(code);
    }

    public Dictionary<string, float> GetVector(string code, int offset)
    {
        return _proxied.GetVector(code, offset + _offset);
    }

    private readonly IDataSource _proxied;
    private readonly int _offset;
}