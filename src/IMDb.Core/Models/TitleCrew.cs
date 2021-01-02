using System;
using System.Linq;

namespace IMDb.Core.Models
{
    /// <summary>
    ///     From the `title.crew.tsv.gz` dataset: contains the director and writer information for all the titles in IMDb.
    /// </summary>
    public class TitleCrew
    {
        /// <summary>
        ///     Alphanumeric unique identifier of the title
        /// </summary>
        public string TConst { get; set; }

        /// <summary>
        ///     Director(s) of the given title
        /// </summary>
        public string[] Directors { get; set; }

        /// <summary>
        ///     Writer(s) of the given title
        /// </summary>
        public string[] Writers { get; set; }

        protected bool Equals(TitleCrew other) => TConst == other.TConst && Directors.SequenceEqual(other.Directors) &&
                                                  Writers.SequenceEqual(other.Writers);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((TitleCrew) obj);
        }

        public override int GetHashCode() => HashCode.Combine(TConst, Directors, Writers);

        public static bool operator ==(TitleCrew left, TitleCrew right) => Equals(left, right);

        public static bool operator !=(TitleCrew left, TitleCrew right) => !Equals(left, right);
    }
}