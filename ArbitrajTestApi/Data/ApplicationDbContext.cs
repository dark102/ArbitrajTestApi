using Microsoft.EntityFrameworkCore;
using ArbitrajTestApi.Models;

namespace ArbitrajTestApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ArbitrageData> ArbitrageData { get; set; }
        public DbSet<FuturesPrice> FuturesPrices { get; set; }
        public DbSet<Log> logs { get; set; }
        public DbSet<TrackedPairs> TrackedPairs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ArbitrageData>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Timestamp).IsRequired();
                entity.Property(e => e.QuarterFutureSymbol).IsRequired();
                entity.Property(e => e.QuarterFuturePrice).IsRequired();
                entity.Property(e => e.BiQuarterFutureSymbol).IsRequired();
                entity.Property(e => e.BiQuarterFuturePrice).IsRequired();
                entity.Property(e => e.PriceDifference).IsRequired();

                entity.HasIndex(e => e.Timestamp)
                    .IsDescending()
                    .HasDatabaseName("IX_ArbitrageData_Timestamp");
            });

            modelBuilder.Entity<FuturesPrice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Symbol).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.Timestamp).IsRequired();

                entity.HasIndex(e => e.Timestamp)
                    .IsDescending()
                    .HasDatabaseName("IX_FuturesPrice_Timestamp");
            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.id);
                entity.Property(e => e.message).IsRequired().HasColumnType("text");
                entity.Property(e => e.level).IsRequired().HasColumnType("text");
                entity.Property(e => e.timestamp).IsRequired().HasColumnType("timestamp with time zone");
                entity.Property(e => e.exception).HasColumnType("text");

                entity.HasIndex(e => e.timestamp)
                    .IsDescending()
                    .HasDatabaseName("IX_Logs_Timestamp");

                entity.HasIndex(e => e.level)
                    .HasDatabaseName("IX_Logs_Level");
            });

            modelBuilder.Entity<TrackedPairs>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.QuarterFutureSymbol).IsRequired();
                entity.Property(e => e.BiQuarterFutureSymbol).IsRequired();
                entity.Property(e => e.isNew).IsRequired();

                entity.HasIndex(t => new { t.QuarterFutureSymbol, t.BiQuarterFutureSymbol })
                    .IsUnique();
            });
        }
    }
}