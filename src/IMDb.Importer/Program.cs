using System;
using System.Threading.Tasks;
using IMDb.Importer;
using Microsoft.Extensions.Logging;
using Serilog;

var databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME") ?? "imdb_db";
using var serilog = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

// Gets the Serilog logger as a Microsoft.Extensions.Logging.ILogger
var logFactory = new LoggerFactory().AddSerilog(serilog);
var logger = logFactory.CreateLogger("IMDb Importer");

logger.LogInformation("Downloading all datasets");
var downloader = new ImdbDatasetImporter(logger);
await downloader.DownloadAndImportAll();

var store = downloader.Store;
using var session = store.QuerySession();

var databaseSize = await session.QueryAsync<string>(
    $"SELECT pg_size_pretty(pg_database_size('{databaseName}'))");

var rowCount =
    await session.QueryAsync<string>(
        "SELECT SUM(n_live_tup) FROM pg_stat_user_tables;");

logger.LogInformation($"Database size: {string.Join(", ", databaseSize)}");
logger.LogInformation($"Table live rows: {string.Join(", ", rowCount)}");

// Until I add scheduling, wait forever
await Task.Delay(-1);