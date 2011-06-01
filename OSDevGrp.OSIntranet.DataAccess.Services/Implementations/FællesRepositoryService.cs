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
    /// Repositoryservice til fælles elementer.
    /// </summary>
    public class FællesRepositoryService : RepositoryServiceBase, IFællesRepositoryService
    {
        #region Private variables

        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repositoryservice til fælles elementer.
        /// </summary>
        /// <param name="logRepository">Implementering af logging repository.</param>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public FællesRepositoryService(ILogRepository logRepository, IQueryBus queryBus)
            : base(logRepository)
        {
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            _queryBus = queryBus;
        }

        #endregion

        #region IFællesRepositoryService Members

        /// <summary>
        /// Henter alle brevhoveder.
        /// </summary>
        /// <param name="brevhovedGetAllQuery">Query til forespørgelse efter alle brevhoveder.</param>
        /// <returns>Alle brevhoveder.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<BrevhovedView> BrevhovedGetAll(BrevhovedGetAllQuery brevhovedGetAllQuery)
        {
            try
            {
                return _queryBus.Query<BrevhovedGetAllQuery, IEnumerable<BrevhovedView>>(brevhovedGetAllQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFællesRepositoryService));
            }
        }

        /// <summary>
        /// Henter et givent brevhoved.
        /// </summary>
        /// <param name="brevhovedGetByNummerQuery">Query til forespørgelse efter et givent brevhovede.</param>
        /// <returns>Brevhoved.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public BrevhovedView BrevhovedGetByNummer(BrevhovedGetByNummerQuery brevhovedGetByNummerQuery)
        {
            try
            {
                return _queryBus.Query<BrevhovedGetByNummerQuery, BrevhovedView>(brevhovedGetByNummerQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFællesRepositoryService));
            }
        }

        #endregion
    }
}
