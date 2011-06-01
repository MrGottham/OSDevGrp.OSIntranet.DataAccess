using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.DataAccess.Infrastructure.Interfaces.Exceptions;
using OSDevGrp.OSIntranet.DataAccess.Resources;
using OSDevGrp.OSIntranet.DataAccess.Services.Domain;

namespace OSDevGrp.OSIntranet.DataAccess.Services.QueryHandlers
{
    /// <summary>
    /// Basisklasse til queryhandlers, der håndterer forespørgelser på regnskaber.
    /// </summary>
    public abstract class RegnskabQueryHandleBase
    {
        #region Methods

        /// <summary>
        /// Samler informationer fra forskellige repositories.
        /// </summary>
        /// <param name="regnskab">Regnskab.</param>
        /// <param name="adresser">Adresser.</param>
        protected void MergeInformations(Regnskab regnskab, IEnumerable<AdresseBase> adresser)
        {
            if (regnskab == null)
            {
                throw new ArgumentNullException("regnskab");
            }
            if (adresser == null)
            {
                throw new ArgumentNullException("adresser");
            }
            var bogføringslinjer = regnskab.Konti
                .OfType<Konto>()
                .SelectMany(m => m.Bogføringslinjer)
                .Where(m => m.Adresse != null && m.Adresse is Adressereference)
                .ToArray();
            foreach (var bogføringslinje in bogføringslinjer)
            {
                var linje = bogføringslinje;
                AdresseBase adresse;
                try
                {
                    adresse = adresser.Single(m => m.Nummer == linje.Adresse.Nummer);
                }
                catch (InvalidOperationException ex)
                {
                    throw new DataAccessSystemException(
                        Resource.GetExceptionMessage(ExceptionMessage.CantFindUniqueRecordId, typeof (AdresseBase),
                                                     linje.Adresse.Nummer), ex);
                }
                adresse.TilføjBogføringslinje(linje);
            }
        }

        #endregion
    }
}
