using System;
using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Finansstyring;

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
        }

        #endregion
    }
}
