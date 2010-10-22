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
    /// Repositoryservice for finansstyring.
    /// </summary>
    public class FinansstyringRepositoryService : RepositoryServiceBase, IFinansstyringRepositoryService
    {
        #region Private variables

        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repositoryservice for finansstyring.
        /// </summary>
        /// <param name="logRepository">Logging repository.</param>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public FinansstyringRepositoryService(ILogRepository logRepository, IQueryBus queryBus)
            : base(logRepository)
        {
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            _queryBus = queryBus;
        }

        #endregion

        #region IFinansstyringRepositoryService Members

        /// <summary>
        /// Henter alle kontogrupper.
        /// </summary>
        /// <param name="kontogruppeGetAllQuery">Forespørgelse til at hente alle kontogrupper.</param>
        /// <returns>Alle kontogrupper.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IList<KontogruppeView> KontogruppeGetAll(KontogruppeGetAllQuery kontogruppeGetAllQuery)
        {
            try
            {
                return _queryBus.Query<KontogruppeGetAllQuery, IList<KontogruppeView>>(kontogruppeGetAllQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFinansstyringRepositoryService));
            }
        }

        /// <summary>
        /// Henter en given kontogruppe.
        /// </summary>
        /// <param name="kontogruppeGetByNummerQuery">Forespørgelse tiol at hente en given kontogruppe.</param>
        /// <returns>Kontogruppe.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public KontogruppeView KontogruppeGetByNummer(KontogruppeGetByNummerQuery kontogruppeGetByNummerQuery)
        {
            try
            {
                return _queryBus.Query<KontogruppeGetByNummerQuery, KontogruppeView>(kontogruppeGetByNummerQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFinansstyringRepositoryService));
            }
        }

        /// <summary>
        /// Henter alle grupper for budgetkonti.
        /// </summary>
        /// <param name="budgetkontogruppeGetAllQuery">Forespørgelse til at hente alle budgetkonti.</param>
        /// <returns>Alle grupper for budgetkonti.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IList<BudgetkontogruppeView> BudgetkontogruppeGetAll(BudgetkontogruppeGetAllQuery budgetkontogruppeGetAllQuery)
        {
            try
            {
                return
                    _queryBus.Query<BudgetkontogruppeGetAllQuery, IList<BudgetkontogruppeView>>(
                        budgetkontogruppeGetAllQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFinansstyringRepositoryService));
            }
        }

        /// <summary>
        /// Henter en given gruppe for budgetkonti.
        /// </summary>
        /// <param name="budgetkontogruppeGetByNummerQuery">Forespørgelse til at en given gruppe for budgetkonti.</param>
        /// <returns>Gruppe for budgetkonti.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public BudgetkontogruppeView BudgetkontoGetByNummer(BudgetkontogruppeGetByNummerQuery budgetkontogruppeGetByNummerQuery)
        {
            try
            {
                return
                    _queryBus.Query<BudgetkontogruppeGetByNummerQuery, BudgetkontogruppeView>(
                        budgetkontogruppeGetByNummerQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFinansstyringRepositoryService));
            }
        }

        #endregion
    }
}
