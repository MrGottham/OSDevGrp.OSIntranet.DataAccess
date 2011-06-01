using System;
using System.Collections.Generic;
using System.Linq;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;
using OSDevGrp.OSIntranet.DataAccess.Services.Domain;

namespace OSDevGrp.OSIntranet.DataAccess.Services.QueryHandlers
{
    /// <summary>
    /// Basisklasse til queryhandlers, der håndterer forespørgelser på adresser.
    /// </summary>
    public abstract class AdresseQueryHandlerBase
    {
        #region Methods

        /// <summary>
        /// Samler informationer fra forskellige repositories.
        /// </summary>
        /// <param name="adresse">Adresse.</param>
        /// <param name="regnskaber">Regnskaber.</param>
        protected void MergeInformations(AdresseBase adresse, IEnumerable<Regnskab> regnskaber)
        {
            if (adresse == null)
            {
                throw new ArgumentNullException("adresse");
            }
            if (regnskaber == null)
            {
                throw new ArgumentNullException("regnskaber");
            }
            var bogføringslinjer = regnskaber
                .SelectMany(m => m.Konti)
                .OfType<Konto>()
                .SelectMany(m => m.Bogføringslinjer)
                .Where(m => m.Adresse != null && m.Adresse is Adressereference && m.Adresse.Nummer == adresse.Nummer)
                .ToArray();
            foreach (var bogføringslinje in bogføringslinjer)
            {
                adresse.TilføjBogføringslinje(bogføringslinje);
            }
        }

        #endregion
    }
}
