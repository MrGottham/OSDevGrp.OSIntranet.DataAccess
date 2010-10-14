using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces
{
    /// <summary>
    /// Repository for adressekartoteket.
    /// </summary>
    public class AdresseRepository : IAdresseRepository
    {
        #region IAdresseRepository Members

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <returns>Liste indeholdende alle adressegrupper.</returns>
        public IList<Adressegruppe> AdressegruppeGetAll()
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}
