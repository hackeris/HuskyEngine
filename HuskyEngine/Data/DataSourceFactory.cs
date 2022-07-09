namespace HuskyEngine.Data;

public class DataSourceFactory
{
    private readonly HuskyDbContext _dbContext;

    public DataSourceFactory(HuskyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IDataSource At(DateTime date)
    {
        var dates = _dbContext.TradingDates
            .Where(td => td.Status == 0)
            .Select(td => td.Date)
            .ToList();
        dates.Sort();

        return new HuskyDataSource(_dbContext, dates, date);
    }
}