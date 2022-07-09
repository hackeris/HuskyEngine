using HuskyEngine.Data.Cache;

namespace HuskyEngine.Data;

public class DataSourceFactory
{
    private readonly HuskyDbContext _dbContext;
    private readonly DataCache _cache;

    public DataSourceFactory(HuskyDbContext dbContext, DataCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public IDataSource At(DateTime date)
    {
        var dates = GetDates();

        return new HuskyDataSource(_dbContext, _cache, dates, date);
    }

    private List<DateTime> GetDates()
    {
        return _cache.GetOrLoad(
            new CacheKey($"DataSourceFactory::{nameof(GetDates)}"),
            () =>
            {
                var dates = _dbContext.TradingDates
                    .Where(td => td.Status == 0)
                    .Select(td => td.Date)
                    .ToList();
                dates.Sort();
                return dates;
            });
    }
}