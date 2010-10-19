using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Domain
{
    /// <summary>
    /// Intern reference til en adresse.
    /// </summary>
    internal class Adressereference : AdresseBase
    {
        #region Constructor

        /// <summary>
        /// Danner en intern reference til en adressen.
        /// </summary>
        /// <param name="nummer">Unik identifikation af adressen.</param>
        public Adressereference(int nummer)
            : base(nummer)
        {
        }

        #endregion
    }
}
