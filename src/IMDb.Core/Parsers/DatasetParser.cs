using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IMDb.Core.Parsers.Models;
using Microsoft.Extensions.Logging;

namespace IMDb.Core.Parsers
{
    public class DatasetParser
    {
        private readonly int _batchSize;
        private readonly ILogger _logger;

        private readonly List<IParser<object>> _parser = new()
        {
            new NameBasicsParser(),
            new TitleAkasParser(),
            new TitleBasicsParser(),
            new TitleCrewParser(),
            new TitleEpisodeParser(),
            new TitlePrincipalsParser(),
            new TitleRatingsParser()
        };

        public DatasetParser(int batchSize, ILogger logger)
        {
            _batchSize = batchSize;
            _logger = logger;
        }

        /// <summary>
        ///     Parses a file line by line, without loading it all into memory at once
        /// </summary>
        /// <param name="stream"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentException"></exception>
        public async IAsyncEnumerable<List<T>> Parse<T>(Stream stream) where T : class
        {
            var parser = (IParser<T>) _parser.FirstOrDefault(x =>
            {
                var interfaces = x.GetType().GetInterfaces();

                if (interfaces.Length != 1)
                {
                    throw new Exception("Expected parser to only implement 1 interface, it had: " + interfaces.Length);
                }

                var interfaceType = interfaces[0];

                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IParser<>))
                {
                    return interfaceType.GetGenericArguments()[0] == typeof(T);
                }

                throw new Exception("Interface doesn't implement IParser<T>");
            });

            if (parser == null)
            {
                throw new ArgumentException("There is no parser for that type", nameof(T));
            }

            var outputType = typeof(T);

            using var streamReader = new StreamReader(stream);

            // The first line is the headers, so ignore that
            await streamReader.ReadLineAsync();

            var lineNumber = 2;

            var batchOutput = new List<T>(_batchSize);

            do
            {
                if (batchOutput.Count >= _batchSize)
                {
                    // Copy such that it won't be a reference
                    _logger.LogInformation($"[{outputType.Name}] Returning batch up to row {lineNumber - 2}");
                    yield return batchOutput;
                    batchOutput = new List<T>(_batchSize);
                }

                var line = await streamReader.ReadLineAsync();

                if (line == null)
                {
                    _logger.LogInformation($"[{outputType.Name}] Finished parsing last batch");
                    yield return batchOutput;
                    break;
                }

                T parsedModel;

                try
                {
                    parsedModel = parser.Parse(line);
                }
                catch (Exception e)
                {
                    _logger.LogError(e,
                        $"[{outputType.Name}] Error with the following line #{lineNumber}: " + Environment.NewLine +
                        line);

                    throw;
                }

                batchOutput.Add(parsedModel);
                lineNumber++;
            } while (true);
        }
    }
}