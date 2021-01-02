using System;

namespace IMDb.Core.Models
{
    /// <summary>
    ///     From the `title.episode.tsv.gz` dataset: contains the tv episode information
    /// </summary>
    public class TitleEpisode
    {
        public string MartenId => TConst + ParentTConst;

        /// <summary>
        ///     Alphanumeric identifier of episode
        /// </summary>
        public string TConst { get; set; }

        /// <summary>
        ///     Alphanumeric identifier of the parent TV Series
        /// </summary>
        public string ParentTConst { get; set; }

        /// <summary>
        ///     Season number the episode belongs to
        /// </summary>
        public int? SeasonNumber { get; set; }

        /// <summary>
        ///     Episode number of the tconst in the TV series
        /// </summary>
        public int? EpisodeNumber { get; set; }

        protected bool Equals(TitleEpisode other) => TConst == other.TConst && ParentTConst == other.ParentTConst &&
                                                     SeasonNumber == other.SeasonNumber &&
                                                     EpisodeNumber == other.EpisodeNumber;

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

            return Equals((TitleEpisode) obj);
        }

        public override int GetHashCode() => HashCode.Combine(TConst, ParentTConst, SeasonNumber, EpisodeNumber);

        public static bool operator ==(TitleEpisode left, TitleEpisode right) => Equals(left, right);

        public static bool operator !=(TitleEpisode left, TitleEpisode right) => !Equals(left, right);
    }
}