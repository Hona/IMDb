using System;
using System.IO;

namespace IMDb.Importer
{
    public static class FileHelper
    {
        public static string GetFileDownloadPath(string file)
        {
            var basePath = Environment.GetEnvironmentVariable("IMDB_BASE_PATH") ?? Constants.ImdbDatasetBasePath;

            return Path.Join(basePath, file);
        }
    }
}