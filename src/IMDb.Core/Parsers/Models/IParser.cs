namespace IMDb.Core.Parsers.Models
{
    public interface IParser<out T>
    {
        public T Parse(string line);
    }
}