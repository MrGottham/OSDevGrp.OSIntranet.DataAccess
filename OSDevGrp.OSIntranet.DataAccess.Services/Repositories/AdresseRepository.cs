using System;
using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Repository for adressekartoteket.
    /// </summary>
    public class AdresseRepository : RepositoryBase, IAdresseRepositoryService
    {
        #region IAdresseRepositoryService Members

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <returns>Alle adressegrupper.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IList<AdressegruppeView> AdressegruppeGetAll()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
