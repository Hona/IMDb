using System;
using System.Linq;

namespace IMDb.Core.Models
{
    /// <summary>
    ///     From the `title.akas.tsv.gz` dataset: the "Title AKAs" - a localised version of titles
    /// </summary>
    public class TitleAKAs
    {
        public string MartenId => TitleId + Ordering;

        /// <summary>
        ///     A tconst, an alphanumeric unique identifier of the title
        /// </summary>
        public string TitleId { get; set; }

        /// <summary>
        ///     A number to uniquely identify rows for a given titleId
        /// </summary>
        public int Ordering { get; set; }

        /// <summary>
        ///     The localized title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     The region for this version of the title
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        ///     The language of the title
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        ///     Enumerated set of attributes for this alternative title. One or more of the following: "alternative", "dvd",
        ///     "festival", "tv", "video", "working", "original", "imdbDisplay". New values may be added in the future without
        ///     warning
        /// </summary>
        public string[] Types { get; set; }

        /// <summary>
        ///     Additional terms to describe this alternative title, not enumerated
        /// </summary>
        public string[] Attributes { get; set; }

        /// <summary>
        ///     0: not original title; 1: original title
        /// </summary>
        public bool? IsOriginalTitle { get; set; }

        protected bool Equals(TitleAKAs other) => TitleId == other.TitleId && Ordering == other.Ordering &&
                                                  Title == other.Title && Region == other.Region &&
                                                  Language == other.Language && Types.SequenceEqual(other.Types) &&
                                                  Attributes.SequenceEqual(other.Attributes) &&
                                                  IsOriginalTitle == other.IsOriginalTitle;

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

            return Equals((TitleAKAs) obj);
        }

        public override int GetHashCode() => HashCode.Combine(TitleId, Ordering, Title, Region, Language, Types,
            Attributes, IsOriginalTitle);

        public static bool operator ==(TitleAKAs left, TitleAKAs right) => Equals(left, right);

        public static bool operator !=(TitleAKAs left, TitleAKAs right) => !Equals(left, right);
    }
}