using System;
using System.Linq;
using System.Reflection;

namespace IMDb.Core.Parsers.Helpers
{
    public static class LineModelValidator
    {
        /// <summary>
        ///     Throws if the number of parts in a line != the expected value, helps to catch invalid data and/or if the dataset
        ///     changes its schema
        /// </summary>
        /// <param name="parts">The split parts</param>
        /// <typeparam name="T">The type to be parsed into</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public static void EnsureCorrectPartLength<T>(this string[] parts) where T : class
        {
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var modelPropertiesLength = properties.Length;

            if (properties.Any(x => x.Name == "MartenId"))
            {
                // If there is a marten ID then subtract one since it is unused for all sakes other than identity
                modelPropertiesLength--;
            }

            if (modelPropertiesLength != parts.Length)
            {
                throw new ArgumentException($"Expected {modelPropertiesLength} parts in the line, got " + parts.Length,
                    nameof(parts));
            }
        }
    }
}