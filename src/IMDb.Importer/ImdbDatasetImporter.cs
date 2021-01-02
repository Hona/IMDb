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

        private readonly string ImdbDatasetsDomain = "datasets.imdbws.com";

        private readonly Dictionary<Type, string> _repositoryUrls;

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

            _repositoryUrls = new Dictionary<Type, string>
            {
                { typeof(NameBasics), $"https://{ImdbDatasetsDomain}/name.basics.tsv.gz" },
                { typeof(TitleAKAs), $"https://{ImdbDatasetsDomain}/title.akas.tsv.gz" },
                { typeof(TitleBasics), $"https://{ImdbDatasetsDomain}/title.basics.tsv.gz" },
                { typeof(TitleCrew), $"https://{ImdbDatasetsDomain}/title.crew.tsv.gz" },
                { typeof(TitleEpisode), $"https://{ImdbDatasetsDomain}/title.episode.tsv.gz" },
                { typeof(TitlePrincipals), $"https://{ImdbDatasetsDomain}/title.principals.tsv.gz" },
                { typeof(TitleRatings), $"https://{ImdbDatasetsDomain}/title.ratings.tsv.gz" },
            };
        }

        public IDocumentStore Store { get; }

        public async Task DownloadAndImportAll()
        {
            var tasks = new List<Task>
            {
                DownloadAndImportType(_nameBasicsRepository),
                DownloadAndImportType(_titleAKAsRepository),
                DownloadAndImportType(_titleBasicsRepository),
                DownloadAndImportType(_titleCrewRepository),
                DownloadAndImportType(_titleEpisodeRepository),
                DownloadAndImportType(_titlePrincipalsRepository),
                DownloadAndImportType(_titleRatingsRepository)
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

        private async Task DownloadAndImportType<T>(AbstractRepository<T> repository, string outputFileName = nameof(T)) where T : class
        {
            var file = await DownloadDataset(_repositoryUrls[typeof(T)], outputFileName);
            await using var fileStream = File.OpenRead(file);

            var batches = _datasetParser.Parse<T>(fileStream);

            await foreach (var batch in batches)
            {
                repository.BulkSync(batch);
            }
        }
    }
}