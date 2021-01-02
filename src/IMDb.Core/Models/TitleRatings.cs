using System;

namespace IMDb.Core.Models
{
    /// <summary>
    ///     From the `title.ratings.tsv.gz` dataset: contains the IMDb rating and votes information for titles
    /// </summary>
    public class TitleRatings
    {
        /// <summary>
        ///     Alphanumeric unique identifier of the title
        /// </summary>
        public string TConst { get; set; }

        /// <summary>
        ///     Weighted average of all the individual user ratings
        /// </summary>
        public decimal AverageRating { get; set; }

        /// <summary>
        ///     Number of votes the title has received
        /// </summary>
        public int TotalVotes { get; set; }

        protected bool Equals(TitleRatings other) => TConst == other.TConst && AverageRating == other.AverageRating &&
                                                     TotalVotes == other.TotalVotes;

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

            return Equals((TitleRatings) obj);
        }

        public override int GetHashCode() => HashCode.Combine(TConst, AverageRating, TotalVotes);

        public static bool operator ==(TitleRatings left, TitleRatings right) => Equals(left, right);

        public static bool operator !=(TitleRatings left, TitleRatings right) => !Equals(left, right);
    }
}