using System.Diagnostics;
using HuskyEngine.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace HuskyEngine.Data;

public class HuskyDataSource : IDataSource
{
    private readonly HuskyDbContext _dbContext;
    private readonly List<DateTime> _dates;
    private readonly DateTime _date;

    public HuskyDataSource(HuskyDbContext dbContext, List<DateTime> dates, DateTime date)
    {
        _dbContext = dbContext;
        _dates = dates;
        _date = date;
    }

    public bool Exist(string code)
    {
        if (code == "zero")
        {
            return true;
        }

        var factor = _dbContext.Factors
            .Single(it => it.Code == code);
        return factor != null;
    }

    public bool IsFormula(string code)
    {
        if (code == "zero")
        {
            return false;
        }

        var factor = _dbContext.Factors
            .Single(it => it.Code == code);
        return factor is { SourceType: 2 };
    }

    public string GetFormula(string code)
    {
        var factor = _dbContext.Factors
            .Single(it => it.Code == code);
        return factor.Formula;
    }

    public Dictionary<string, float> GetVector(string code, int offset)
    {
        Debug.Assert(offset <= 0);

        if (code == "zero")
        {
            return GetSymbols()
                .ToDictionary(s => s, _ => 0.0f);
        }

        var factor = _dbContext.Factors
            .Single(it => it.Code == code);

        var dateIndex = _dates.FindLastIndex(e => e <= _date);

        var factorId = factor.Id;

        if (factor.SourceType == 0)
        {
            var exactDate = _dates[dateIndex + offset];
            return _dbContext.FactorData
                .Where(dt =>
                    dt.Id == factorId
                    && dt.Date == exactDate
                    && dt.Value != null)
                .ToDictionary(dt => dt.Symbol, dt => (float)dt.Value!);
        }

        if (factor.SourceType == 1)
        {
            var referenceDate = _dates[dateIndex];
            var queryOffset = -offset;
            var symbols = GetSymbols();
            return symbols
                .Chunk(100)
                .Select(chunk => SelectFinancialDatum(factorId, chunk, referenceDate, queryOffset))
                .Aggregate((a, b) => a.Union(b).ToList())
                .Where(p => p.Value != null)
                .ToDictionary(p => p.Symbol, p => (float)p.Value!);
        }

        throw new Exception("Unsupported");
    }

    private List<FinancialFactorDatum> SelectFinancialDatum(
        long factorId,
        string[] symbols,
        DateTime referenceDate,
        int queryOffset
    )
    {
        var dateString = referenceDate.ToString("s");

        var sql = symbols
            .Select(symbol => String.Format(
                @"SELECT 
                    * 
                FROM financial_factor_data 
                WHERE `id` = {0} 
                    AND `symbol` = '{1}' 
                    AND `releaseDate` <= '{2}'
                    AND `releaseDate` >= DATE_SUB('{2}', INTERVAL {3} MONTH) 
                ORDER BY `endDate` DESC LIMIT {4}, 1",
                factorId,
                symbol,
                dateString,
                queryOffset * 3 + 12,
                queryOffset
            ))
            .Select(s => $"({s})")
            .Aggregate((a, b) => $"{a} UNION {b}");

        return _dbContext.FinancialFactorData
            .FromSqlRaw(sql)
            .ToList();
    }

    private List<string> GetSymbols()
    {
        var closePrices = GetVector("close_price_backward", 0);
        return closePrices
            .Select(p => p.Key)
            .ToList();
    }
}