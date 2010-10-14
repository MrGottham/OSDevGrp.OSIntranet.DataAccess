using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Services;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;
using OSDevGrp.OSIntranet.DataAccess.Services.Repositories.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Implementations
{
    /// <summary>
    /// Repositoryservice for adressekartoteket.
    /// </summary>
    public class AdresseRepositoryService : RepositoryServiceBase, IAdresseRepositoryService
    {
        #region Private variables

        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repositoryservice for addressekartoteket.
        /// </summary>
        /// <param name="logRepository">Implementering af logging repository.</param>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public AdresseRepositoryService(ILogRepository logRepository, IQueryBus queryBus)
            : base(logRepository)
        {
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            _queryBus = queryBus;
        }

        #endregion

        #region IAdresseRepositoryService Members

        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <param name="adressegruppeGetAllQuery">Query til forespørgelse efter alle adressegrupper.</param>
        /// <returns>Alle adressegrupper.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IList<AdressegruppeView> AdressegruppeGetAll(AdressegruppeGetAllQuery adressegruppeGetAllQuery)
        {
            try
            {
                return _queryBus.Query<AdressegruppeGetAllQuery, IList<AdressegruppeView>>(adressegruppeGetAllQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogAdresseRepositoryService));
            }
        }

        #endregion
    }
}
