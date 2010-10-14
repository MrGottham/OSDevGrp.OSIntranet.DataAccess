using System;
using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Repository for adressekartoteket.
    /// </summary>
    public class AdresseRepository : RepositoryBase, IAdresseRepositoryService
    {
        #region Private variables

        private readonly ILogRepository _logRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repository for addressekartoteket.
        /// </summary>
        /// <param name="logRepository">Implementering af logging repository.</param>
        public AdresseRepository(ILogRepository logRepository)
        {
            if (logRepository == null)
            {
                throw new ArgumentNullException("logRepository");
            }
            _logRepository = logRepository;
        }

        #endregion

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
