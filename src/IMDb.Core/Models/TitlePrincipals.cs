using System;

namespace IMDb.Core.Models
{
    /// <summary>
    ///     From the `title.principals.tsv.gz` dataset: contains the principal cast/crew for titles
    /// </summary>
    public class TitlePrincipals
    {
        public string MartenId => TConst + Ordering;

        /// <summary>
        ///     Alphanumeric unique identifier of the title
        /// </summary>
        public string TConst { get; set; }

        /// <summary>
        ///     A number to uniquely identify rows for a given titleId
        /// </summary>
        public int Ordering { get; set; }

        /// <summary>
        ///     Alphanumeric unique identifier of the name/person
        /// </summary>
        public string NConst { get; set; }

        /// <summary>
        ///     The category of job that person was in
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        ///     The specific job title if applicable, else '\N'
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        ///     The name of the character played if applicable, else '\N'
        /// </summary>
        public string Character { get; set; }

        protected bool Equals(TitlePrincipals other) => TConst == other.TConst && Ordering == other.Ordering &&
                                                        NConst == other.NConst && Category == other.Category &&
                                                        Job == other.Job && Character == other.Character;

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

            return Equals((TitlePrincipals) obj);
        }

        public override int GetHashCode() => HashCode.Combine(TConst, Ordering, NConst, Category, Job, Character);

        public static bool operator ==(TitlePrincipals left, TitlePrincipals right) => Equals(left, right);

        public static bool operator !=(TitlePrincipals left, TitlePrincipals right) => !Equals(left, right);
    }
}