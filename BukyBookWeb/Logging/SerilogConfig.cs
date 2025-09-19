using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;

namespace BukyBookWeb.Logging
{
    public static class SerilogConfig
    {
        public static void ConfigureSerilog(WebApplicationBuilder builder)
        {
            var columnOptions = new ColumnOptions();
            columnOptions.AdditionalColumns = new Collection<SqlColumn>
            {
                new SqlColumn("LogGuid", System.Data.SqlDbType.UniqueIdentifier)
            };

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(
                    path: "Logs/log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    restrictedToMinimumLevel: LogEventLevel.Error,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .WriteTo.MSSqlServer(
                    connectionString: builder.Configuration.GetConnectionString("DefultConnection"),
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Logs",
                        AutoCreateSqlTable = true
                    },
                    columnOptions: columnOptions,
                    restrictedToMinimumLevel: LogEventLevel.Error
                )
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }

}
