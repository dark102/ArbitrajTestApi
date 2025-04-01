using ArbitrajTestApi.Data;
using ArbitrajTestApi.Jobs;
using ArbitrajTestApi.Repositories;
using ArbitrajTestApi.Services;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ArbitrajTestApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Настройка Entity Framework
            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(migrationsAssembly));
            });

            // Настройка Hangfire
            builder.Services.AddHangfire(config =>
                config.UsePostgreSqlStorage(options =>
                    options.UseNpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection"))));
            builder.Services.AddHangfireServer();

            // Добавление сервисов
            builder.Services.AddControllers();

            // Настройка Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ArbitrajTestApi",
                    Version = "v1",
                    Description = "API for tracking and calculating arbitrage opportunities"
                });
            });

            // Регистрация сервисов
            builder.Services.AddScoped<IFuturesService, FuturesService>();
            builder.Services.AddScoped<IArbitrageService, ArbitrageService>();
            builder.Services.AddScoped<IArbitrageRepository, ArbitrageRepository>();
            builder.Services.AddScoped<ITrackedPairsRepository, TrackedPairsRepository>();
            builder.Services.AddScoped<ArbitrageJob>();

            // Настройка Serilog
            builder.Logging.ClearProviders();
            builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
                .ReadFrom.Configuration(hostingContext.Configuration)
                .WriteTo.Console()
                .WriteTo.PostgreSQL(
                    connectionString: hostingContext.Configuration.GetConnectionString("DefaultConnection"),
                    tableName: "Logs",
                    columnOptions: new Dictionary<string, ColumnWriterBase>
                    {
                        { "Message", new RenderedMessageColumnWriter() },
                        { "MessageTemplate", new MessageTemplateColumnWriter() },
                        { "Level", new LevelColumnWriter(true) },
                        { "Timestamp", new TimestampColumnWriter( NpgsqlTypes.NpgsqlDbType.Timestamp) },
                        { "Exception", new ExceptionColumnWriter() },
                        { "Properties", new PropertiesColumnWriter() }
                    }));

            var app = builder.Build();

            // Накатываем миграции
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            // Настройка Swagger (включил на постоянку. Потом убрать)
            // if (app.Environment.IsDevelopment())
            // {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ArbitrajTestApi v1");
                options.RoutePrefix = string.Empty;
            });
            //}

            app.UseSerilogRequestLogging();
            app.UseHangfireDashboard();
            app.UseAuthorization();
            app.MapControllers();

            // Запуск инициализации исторических данных при первом запуске
            using (var scope = app.Services.CreateScope())
            {
                var arbitrageJob = scope.ServiceProvider.GetRequiredService<ArbitrageJob>();
                await arbitrageJob.CalculateArbitrage(true);
            }

            // Настройка периодических задач
            RecurringJob.AddOrUpdate<ArbitrageJob>("ArbitrageJob",
                job => job.CalculateArbitrage(false),
                "0 * * * * *",
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.Utc
                });

            app.Run();
        }
    }
}
