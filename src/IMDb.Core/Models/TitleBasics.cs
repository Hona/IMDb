using System;
using System.Linq;

namespace IMDb.Core.Models
{
    /// <summary>
    ///     From the `title.basics.tsv.gz` dataset: the basic info on titles
    /// </summary>
    public class TitleBasics
    {
        /// <summary>
        ///     Alphanumeric unique identifier of the title
        /// </summary>
        public string TConst { get; set; }

        /// <summary>
        ///     The type/format of the title (e.g. movie, short, tvseries, tvepisode, video, etc)
        /// </summary>
        public string TitleType { get; set; }

        /// <summary>
        ///     The more popular title / the title used by the filmmakers on promotional materials at the point of release
        /// </summary>
        public string PrimaryTitle { get; set; }

        /// <summary>
        ///     Original title, in the original language
        /// </summary>
        public string OriginalTitle { get; set; }

        /// <summary>
        ///     0: non-adult title; 1: adult title
        /// </summary>
        public bool? IsAdult { get; set; }

        /// <summary>
        ///     Represents the release year of a title. In the case of TV Series, it is the series start year
        /// </summary>
        public int? StartYear { get; set; }

        /// <summary>
        ///     TV Series end year. ‘\N’ for all other title types
        /// </summary>
        public int? EndYear { get; set; }

        /// <summary>
        ///     Primary runtime of the title, in minutes
        /// </summary>
        public int? RuntimeMinutes { get; set; }

        /// <summary>
        ///     Includes up to three genres associated with the title
        /// </summary>
        public string[] Genres { get; set; }

        protected bool Equals(TitleBasics other) => TConst == other.TConst && TitleType == other.TitleType &&
                                                    PrimaryTitle == other.PrimaryTitle &&
                                                    OriginalTitle == other.OriginalTitle && IsAdult == other.IsAdult &&
                                                    StartYear == other.StartYear && EndYear == other.EndYear &&
                                                    RuntimeMinutes == other.RuntimeMinutes &&
                                                    Genres.SequenceEqual(other.Genres);

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

            return Equals((TitleBasics) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(TConst);
            hashCode.Add(TitleType);
            hashCode.Add(PrimaryTitle);
            hashCode.Add(OriginalTitle);
            hashCode.Add(IsAdult);
            hashCode.Add(StartYear);
            hashCode.Add(EndYear);
            hashCode.Add(RuntimeMinutes);
            hashCode.Add(Genres);
            return hashCode.ToHashCode();
        }

        public static bool operator ==(TitleBasics left, TitleBasics right) => Equals(left, right);

        public static bool operator !=(TitleBasics left, TitleBasics right) => !Equals(left, right);
    }
}