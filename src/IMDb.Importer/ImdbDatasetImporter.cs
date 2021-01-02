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

        // These could be refactored to have the shared logic in one spot, but its not that much code, so I don't mind for now

        #region Individual Dataset Import Functions

        private async Task DownloadAndImportNameBasics()
        {
            var file = await DownloadDataset("https://datasets.imdbws.com/name.basics.tsv.gz", nameof(NameBasics));
            await using var fileStream = File.OpenRead(file);

            var batches = _datasetParser.Parse<NameBasics>(fileStream);

            await foreach (var batch in batches)
            {
                _nameBasicsRepository.BulkSync(batch);
            }
        }

        private async Task DownloadAndImportTitleAKAs()
        {
            var file = await DownloadDataset("https://datasets.imdbws.com/title.akas.tsv.gz", nameof(TitleAKAs));
            await using var fileStream = File.OpenRead(file);

            var batches = _datasetParser.Parse<TitleAKAs>(fileStream);

            await foreach (var batch in batches)
            {
                _titleAKAsRepository.BulkSync(batch);
            }
        }

        private async Task DownloadAndImportTitleBasics()
        {
            var file = await DownloadDataset("https://datasets.imdbws.com/title.basics.tsv.gz", nameof(TitleBasics));
            await using var fileStream = File.OpenRead(file);

            var batches = _datasetParser.Parse<TitleBasics>(fileStream);

            await foreach (var batch in batches)
            {
                _titleBasicsRepository.BulkSync(batch);
            }
        }

        private async Task DownloadAndImportTitleCrew()
        {
            var file = await DownloadDataset("https://datasets.imdbws.com/title.crew.tsv.gz", nameof(TitleCrew));
            await using var fileStream = File.OpenRead(file);

            var batches = _datasetParser.Parse<TitleCrew>(fileStream);

            await foreach (var batch in batches)
            {
                _titleCrewRepository.BulkSync(batch);
            }
        }

        private async Task DownloadAndImportTitleEpisode()
        {
            var file = await DownloadDataset("https://datasets.imdbws.com/title.episode.tsv.gz", nameof(TitleEpisode));
            await using var fileStream = File.OpenRead(file);

            var batches = _datasetParser.Parse<TitleEpisode>(fileStream);

            await foreach (var batch in batches)
            {
                _titleEpisodeRepository.BulkSync(batch);
            }
        }

        private async Task DownloadAndImportTitlePrincipals()
        {
            var file = await DownloadDataset("https://datasets.imdbws.com/title.principals.tsv.gz",
                nameof(TitlePrincipals));
            await using var fileStream = File.OpenRead(file);

            var batches = _datasetParser.Parse<TitlePrincipals>(fileStream);

            await foreach (var batch in batches)
            {
                _titlePrincipalsRepository.BulkSync(batch);
            }
        }

        private async Task DownloadAndImportTitleRatings()
        {
            var file = await DownloadDataset("https://datasets.imdbws.com/title.ratings.tsv.gz", nameof(TitleRatings));
            await using var fileStream = File.OpenRead(file);

            var batches = _datasetParser.Parse<TitleRatings>(fileStream);

            await foreach (var batch in batches)
            {
                _titleRatingsRepository.BulkSync(batch);
            }
        }

        #endregion
    }
}