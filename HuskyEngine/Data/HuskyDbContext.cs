#nullable disable

using HuskyEngine.Data.Model;
using Microsoft.EntityFrameworkCore;
using Task = HuskyEngine.Data.Model.Task;

namespace HuskyEngine.Data;

public class HuskyDbContext : DbContext
{
    public HuskyDbContext()
    {
    }

    public HuskyDbContext(DbContextOptions<HuskyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Factor> Factors { get; set; }
    public virtual DbSet<FactorDatum> FactorData { get; set; }
    public virtual DbSet<FinancialFactorDatum> FinancialFactorData { get; set; }
    public virtual DbSet<Stock> Stocks { get; set; }
    public virtual DbSet<StockHolding> StockHoldings { get; set; }
    public virtual DbSet<Task> Tasks { get; set; }
    public virtual DbSet<TradingDate> TradingDates { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Factor>(entity =>
        {
            entity.ToTable("factor");

            entity.HasIndex(e => e.Code, "code")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("code");

            entity.Property(e => e.DataEnd)
                .HasColumnType("datetime(6)")
                .HasColumnName("dataEnd");

            entity.Property(e => e.DataStart)
                .HasColumnType("datetime(6)")
                .HasColumnName("dataStart");

            entity.Property(e => e.Formula)
                .HasMaxLength(1024)
                .HasColumnName("formula");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("name");

            entity.Property(e => e.SourceType).HasColumnName("sourceType");

            entity.Property(e => e.Type).HasColumnName("type");

            entity.Property(e => e.ValueType).HasColumnName("valueType");
        });

        modelBuilder.Entity<FactorDatum>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Date, e.Symbol })
                .HasName("PRIMARY");

            entity.ToTable("factor_data");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Date)
                .HasColumnType("datetime(6)")
                .HasColumnName("date");

            entity.Property(e => e.Symbol)
                .HasMaxLength(16)
                .HasColumnName("symbol");

            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<FinancialFactorDatum>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Symbol, e.EndDate })
                .HasName("PRIMARY");

            entity.ToTable("financial_factor_data");

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Symbol)
                .HasMaxLength(16)
                .HasColumnName("symbol");

            entity.Property(e => e.EndDate)
                .HasColumnType("datetime(6)")
                .HasColumnName("endDate");

            entity.Property(e => e.ReleaseDate)
                .HasColumnType("datetime(6)")
                .HasColumnName("releaseDate");

            entity.Property(e => e.Value).HasColumnName("value");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Symbol)
                .HasName("PRIMARY");

            entity.ToTable("stock");

            entity.Property(e => e.Symbol)
                .HasMaxLength(16)
                .HasColumnName("symbol");

            entity.Property(e => e.EndDate)
                .HasColumnType("datetime(6)")
                .HasColumnName("endDate");

            entity.Property(e => e.Name)
                .HasMaxLength(32)
                .HasColumnName("name");

            entity.Property(e => e.StartDate)
                .HasColumnType("datetime(6)")
                .HasColumnName("startDate");
        });

        modelBuilder.Entity<StockHolding>(entity =>
        {
            entity.HasKey(e => new { e.Code, e.Date, e.Symbol })
                .HasName("PRIMARY");

            entity.ToTable("stock_holding");

            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .HasColumnName("code");

            entity.Property(e => e.Date)
                .HasColumnType("datetime(6)")
                .HasColumnName("date");

            entity.Property(e => e.Symbol)
                .HasMaxLength(255)
                .HasColumnName("symbol");

            entity.Property(e => e.Weight).HasColumnName("weight");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.ToTable("task");

            entity.Property(e => e.Id)
                .HasMaxLength(255)
                .HasColumnName("id");

            entity.Property(e => e.Parameters).HasColumnName("parameters");

            entity.Property(e => e.Progress).HasColumnName("progress");

            entity.Property(e => e.TaskResult)
                .HasColumnType("longtext")
                .HasColumnName("taskResult");

            entity.Property(e => e.TaskStatus).HasColumnName("taskStatus");

            entity.Property(e => e.TaskType)
                .HasMaxLength(255)
                .HasColumnName("taskType");

            entity.Property(e => e.UserId).HasColumnName("userId");
        });

        modelBuilder.Entity<TradingDate>(entity =>
        {
            entity.HasKey(e => e.Date)
                .HasName("PRIMARY");

            entity.ToTable("trading_date");

            entity.Property(e => e.Date)
                .HasColumnType("datetime(6)")
                .HasColumnName("date");

            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.HasIndex(e => e.Username, "UK_sb8bbouer5wak8vyiiy4pf2bx")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");

            entity.Property(e => e.SocketToken)
                .HasMaxLength(255)
                .HasColumnName("socketToken");

            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("username");
        });

    }

}