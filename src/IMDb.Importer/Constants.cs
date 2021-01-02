using System;
using System.IO;

namespace IMDb.Importer
{
    public static class Constants
    {
        public const int BatchSize = 50000;
        public static readonly string ImdbDatasetBasePath = Path.Join(Environment.CurrentDirectory, "imdb");
    }
}