using System.Collections.Generic;
using OSDevGrp.OSIntranet.CommonLibrary.Domain.Adressekartotek;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Repository for adressekartoteket.
    /// </summary>
    public class AdresseRepository : DbAxRepositoryBase, IAdresseRepository
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
