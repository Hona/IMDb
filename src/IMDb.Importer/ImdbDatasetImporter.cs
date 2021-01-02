using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using IMDb.Core.Models;
using IMDb.Core.Parsers;
using IMDb.Infrastructure.Extensions;
using IMDb.Infrastructure.Repositories;
using Marten;
using Microsoft.Extensions.Logging;

namespace IMDb.Importer
{
    public class ImdbDatasetImporter
    {
        private readonly DatasetParser _datasetParser;

        private readonly HttpClient _httpClient = new();

        private readonly ILogger _logger;

        private readonly NameBasicsRepository _nameBasicsRepository;
        private readonly TitleAKAsRepository _titleAKAsRepository;
        private readonly TitleBasicsRepository _titleBasicsRepository;
        private readonly TitleCrewRepository _titleCrewRepository;
        private readonly TitleEpisodeRepository _titleEpisodeRepository;
        private readonly TitlePrincipalsRepository _titlePrincipalsRepository;
        private readonly TitleRatingsRepository _titleRatingsRepository;

        public ImdbDatasetImporter(ILogger logger)
        {
            _logger = logger;

            _logger.LogInformation("Getting connection string");
            var connectionString = Environment.GetEnvironmentVariable("MARTEN_CONNECTION_STRING") ??
                                   throw new Exception("No connection string passed in to env vars");

            var batchSizeEnvVar = Environment.GetEnvironmentVariable("BATCH_SIZE");
            var batchSize = batchSizeEnvVar == null ? Constants.BatchSize : int.Parse(batchSizeEnvVar);
            _logger.LogInformation("Using batch size of " + batchSize);

            _datasetParser = new DatasetParser(batchSize, _logger);

            Store = DocumentStore.For(options =>
            {
                options.AutoCreateSchemaObjects = AutoCreate.All;

                options.Connection(connectionString);

                options.SetupIMDbIdentities();
            });

            _logger.LogInformation("Setting up repositories");

            _nameBasicsRepository = new NameBasicsRepository(Store);
            _titleAKAsRepository = new TitleAKAsRepository(Store);
            _titleBasicsRepository = new TitleBasicsRepository(Store);
            _titleCrewRepository = new TitleCrewRepository(Store);
            _titleEpisodeRepository = new TitleEpisodeRepository(Store);
            _titlePrincipalsRepository = new TitlePrincipalsRepository(Store);
            _titleRatingsRepository = new TitleRatingsRepository(Store);
        }

        public IDocumentStore Store { get; }

        public async Task DownloadAndImportAll()
        {
            var tasks = new List<Task>
            {
                DownloadAndImportNameBasics(),
                DownloadAndImportTitleAKAs(),
                DownloadAndImportTitleBasics(),
                DownloadAndImportTitleCrew(),
                DownloadAndImportTitleEpisode(),
                DownloadAndImportTitlePrincipals(),
                DownloadAndImportTitleRatings()
            };

            await Task.WhenAll(tasks);
        }

        /// <summary>
        ///     Downloads the IMDb dataset from the site, if the env var `SKIP_DOWNLOAD` is set, then a cold run is done - ideal
        ///     for debugging/testing
        /// </summary>
        /// <returns>The full path to the downloaded dataset</returns>
        private async Task<string> DownloadDataset(string url, string outputFileName)
        {
            var fileName = FileHelper.GetFileDownloadPath(outputFileName);

            if (Environment.GetEnvironmentVariable("SKIP_DOWNLOAD") != null && File.Exists(fileName))
            {
                // Skip downloading, serve from old version on disk
                _logger.LogWarning(
                    "Skipping file download, old version on disk. Remove env var SKIP_DOWNLOAD to get new versions");
                return fileName;
            }

            _logger.LogInformation($"Downloading '{url}'");
            var compressedStream = await _httpClient.GetStreamAsync(url);

            await using var outputFileStream = File.Create(fileName);
            await using var decompressionStream = new GZipStream(compressedStream, CompressionMode.Decompress);

            await decompressionStream.CopyToAsync(outputFileStream);
            _logger.LogInformation($"Done writing '{fileName}' to disk");

            return fileName;
        }

        #region Individual Dataset Import Functions

        private async Task DownloadAndImportNameBasics()
        {
            await DownloadAndImportType<NameBasics>(_nameBasicsRepository);
        }

        private async Task DownloadAndImportTitleAKAs()
        {
            await DownloadAndImportType<TitleAKAs>(_titleAKAsRepository);
        }

        private async Task DownloadAndImportTitleBasics()
        {
            await DownloadAndImportType<TitleBasics>(_titleBasicsRepository);
        }

        private async Task DownloadAndImportTitleCrew()
        {
            await DownloadAndImportType<TitleCrew>(_titleCrewRepository);
        }

        private async Task DownloadAndImportTitleEpisode()
        {
            await DownloadAndImportType<TitleEpisode>(_titleEpisodeRepository);
        }

        private async Task DownloadAndImportTitlePrincipals()
        {
            await DownloadAndImportType<TitlePrincipals>(_titlePrincipalsRepository);
        }

        private async Task DownloadAndImportTitleRatings()
        {
            await DownloadAndImportType<TitleRatings>(_titleRatingsRepository);
        }

        private async Task DownloadAndImportType<T>(AbstractRepository<T> repository, string outputFileName = nameof(T)) where T : class
        {
            var file = await DownloadDataset(repository.Url, outputFileName);
            await using var fileStream = File.OpenRead(file);

            var batches = _datasetParser.Parse<T>(fileStream);

            await foreach (var batch in batches)
            {
                repository.BulkSync(batch);
            }
        }

        #endregion
    }
}