using System;

namespace IMDb.Core.Models
{
    /// <summary>
    ///     From the `name.basics.tsv.gz` dataset: Contains basic information on names
    /// </summary>
    public class NameBasics
    {
        /// <summary>
        ///     Alphanumeric unique identifier of the name/person
        /// </summary>
        public string NConst { get; set; }

        /// <summary>
        ///     Name by which the person is most often credited
        /// </summary>
        public string PrimaryName { get; set; }

        /// <summary>
        ///     In YYYY format
        /// </summary>
        public int? BirthYear { get; set; }

        /// <summary>
        ///     In YYYY format if applicable, else '\N'
        /// </summary>
        public int? DeathYear { get; set; }

        /// <summary>
        ///     The top-3 professions of the person
        /// </summary>
        public string[] PrimaryProfession { get; set; }

        /// <summary>
        ///     titles the person is known for, as `tconsts`
        /// </summary>
        public string[] KnownForTitles { get; set; }

        protected bool Equals(NameBasics other) => NConst == other.NConst && PrimaryName == other.PrimaryName &&
                                                   BirthYear == other.BirthYear && DeathYear == other.DeathYear &&
                                                   Equals(PrimaryProfession, other.PrimaryProfession) &&
                                                   Equals(KnownForTitles, other.KnownForTitles);

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

            return Equals((NameBasics) obj);
        }

        public override int GetHashCode() =>
            HashCode.Combine(NConst, PrimaryName, BirthYear, DeathYear, PrimaryProfession, KnownForTitles);

        public static bool operator ==(NameBasics left, NameBasics right) => Equals(left, right);

        public static bool operator !=(NameBasics left, NameBasics right) => !Equals(left, right);
    }
}