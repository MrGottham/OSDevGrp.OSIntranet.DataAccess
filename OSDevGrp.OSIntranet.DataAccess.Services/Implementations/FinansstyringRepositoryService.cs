using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
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

        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;

        #endregion

        #region Constructor

        /// <summary>
        /// Danner repositoryservice for finansstyring.
        /// </summary>
        /// <param name="logRepository">Logging repository.</param>
        /// <param name="commandBus">Implementering af en CommandBus.</param>
        /// <param name="queryBus">Implementering af en QueryBus.</param>
        public FinansstyringRepositoryService(ILogRepository logRepository, ICommandBus commandBus, IQueryBus queryBus)
            : base(logRepository)
        {
            if (commandBus == null)
            {
                throw new ArgumentNullException("commandBus");
            }
            if (queryBus == null)
            {
                throw new ArgumentNullException("queryBus");
            }
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        #endregion

        #region IFinansstyringRepositoryService Members

        /// <summary>
        /// Henter alle regnskaber.
        /// </summary>
        /// <param name="regnskabGetAllQuery">Forespørgelse til at hente alle regnskaber.</param>
        /// <returns>Alle regnskaber.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<RegnskabListeView> RegnskabGetAll(RegnskabGetAllQuery regnskabGetAllQuery)
        {
            try
            {
                return _queryBus.Query<RegnskabGetAllQuery, IEnumerable<RegnskabListeView>>(regnskabGetAllQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFinansstyringRepositoryService));
            }
        }

        /// <summary>
        /// Henter et givent regnskab.
        /// </summary>
        /// <param name="regnskabGetByNummerQuery">Forespørgelse til at hente et givent regnskab.</param>
        /// <returns>Regnskab.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public RegnskabView RegnskabGetByNummer(RegnskabGetByNummerQuery regnskabGetByNummerQuery)
        {
            try
            {
                return _queryBus.Query<RegnskabGetByNummerQuery, RegnskabView>(regnskabGetByNummerQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFinansstyringRepositoryService));
            }
        }

        /// <summary>
        /// Henter alle konti i et givent regnskab.
        /// </summary>
        /// <param name="kontoGetByRegnskabQuery">Forespørgelse til at hente alle konti i et givent regnskab.</param>
        /// <returns>Alle konti i regnskabet.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<KontoListeView> KontoGetByRegnskab(KontoGetByRegnskabQuery kontoGetByRegnskabQuery)
        {
            try
            {
                return _queryBus.Query<KontoGetByRegnskabQuery, IEnumerable<KontoListeView>>(kontoGetByRegnskabQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFinansstyringRepositoryService));
            }
        }

        /// <summary>
        /// Henter en given konto i et givent regnskab.
        /// </summary>
        /// <param name="kontoGetByRegnskabAndKontonummerQuery">Forespørgelse til at hente en given konto i et givent regnskab.</param>
        /// <returns>Konto.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public KontoView KontoGetByRegnskabAndKontonummer(KontoGetByRegnskabAndKontonummerQuery kontoGetByRegnskabAndKontonummerQuery)
        {
            try
            {
                return
                    _queryBus.Query<KontoGetByRegnskabAndKontonummerQuery, KontoView>(
                        kontoGetByRegnskabAndKontonummerQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFinansstyringRepositoryService));
            }
        }

        /// <summary>
        /// Henter alle budgetkonti i et givent regnskab.
        /// </summary>
        /// <param name="budgetkontoGetByRegnskabQuery">Forespørgelse til at hente alle budgetkonti i et givent regnskab.</param>
        /// <returns>Alle budgetkonti i et givent regnskab.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<BudgetkontoListeView> BudgetkontoGetByRegnskab(BudgetkontoGetByRegnskabQuery budgetkontoGetByRegnskabQuery)
        {
            try
            {
                return
                    _queryBus.Query<BudgetkontoGetByRegnskabQuery, IEnumerable<BudgetkontoListeView>>(
                        budgetkontoGetByRegnskabQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFinansstyringRepositoryService));
            }
        }

        /// <summary>
        /// Henter en given budgetkonto i et givent regnskab.
        /// </summary>
        /// <param name="budgetkontoGetByRegnskabAndKontonummerQuery">Forespørgelse til at hente en given budgetkonto i et givent regnskab.</param>
        /// <returns>Budgetkonto.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public BudgetkontoView BudgetkontoGetByRegnskabAndKontonummer(BudgetkontoGetByRegnskabAndKontonummerQuery budgetkontoGetByRegnskabAndKontonummerQuery)
        {
            try
            {
                return
                    _queryBus.Query<BudgetkontoGetByRegnskabAndKontonummerQuery, BudgetkontoView>(
                        budgetkontoGetByRegnskabAndKontonummerQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFinansstyringRepositoryService));
            }
        }

        /// <summary>
        /// Henter alle bogføringslinjer for et givent regnskab.
        /// </summary>
        /// <param name="bogføringslinjeGetByRegnskabQuery">Forespørgelse til at hente alle bogføringslinjer for et givent regnskab.</param>
        /// <returns>Alle bogføringslinjer for regnskabet.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<BogføringslinjeView> BogføringslinjeGetByRegnskab(BogføringslinjeGetByRegnskabQuery bogføringslinjeGetByRegnskabQuery)
        {
            try
            {
                return
                    _queryBus.Query<BogføringslinjeGetByRegnskabQuery, IEnumerable<BogføringslinjeView>>(
                        bogføringslinjeGetByRegnskabQuery);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFinansstyringRepositoryService));
            }
        }

        /// <summary>
        /// Tilføjer en bogføringslinje.
        /// </summary>
        /// <param name="bogføringslinjeAddCommand">Kommando til tilføjelse af en bogføringslinje.</param>
        [OperationBehavior(TransactionScopeRequired = false)]
        public void BogføringslinjeAdd(BogføringslinjeAddCommand bogføringslinjeAddCommand)
        {
            try
            {
                _commandBus.Publish(bogføringslinjeAddCommand);
            }
            catch (Exception ex)
            {
                throw CreateFault(MethodBase.GetCurrentMethod(), ex,
                                  int.Parse(Properties.Resources.EventLogFinansstyringRepositoryService));
            }
        }

        /// <summary>
        /// Henter alle kontogrupper.
        /// </summary>
        /// <param name="kontogruppeGetAllQuery">Forespørgelse til at hente alle kontogrupper.</param>
        /// <returns>Alle kontogrupper.</returns>
        [OperationBehavior(TransactionScopeRequired = false)]
        public IEnumerable<KontogruppeView> KontogruppeGetAll(KontogruppeGetAllQuery kontogruppeGetAllQuery)
        {
            try
            {
                return _queryBus.Query<KontogruppeGetAllQuery, IEnumerable<KontogruppeView>>(kontogruppeGetAllQuery);
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
        public IEnumerable<BudgetkontogruppeView> BudgetkontogruppeGetAll(BudgetkontogruppeGetAllQuery budgetkontogruppeGetAllQuery)
        {
            try
            {
                return _queryBus.Query<BudgetkontogruppeGetAllQuery, IEnumerable<BudgetkontogruppeView>>(budgetkontogruppeGetAllQuery);
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
        public BudgetkontogruppeView BudgetkontogruppeGetByNummer(BudgetkontogruppeGetByNummerQuery budgetkontogruppeGetByNummerQuery)
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
