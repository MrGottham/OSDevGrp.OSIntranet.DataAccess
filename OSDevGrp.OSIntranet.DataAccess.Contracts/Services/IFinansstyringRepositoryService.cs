using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til repository for finansstyring.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.FinansstyringRepositoryServiceName, Namespace = SoapNamespaces.DataAccessNamespace)]
    public interface IFinansstyringRepositoryService : IRepositoryService
    {
        /// <summary>
        /// Henter alle konti i et givent regnskab.
        /// </summary>
        /// <param name="kontoGetByRegnskabQuery">Forespørgelse til at hente alle konti i et givent regnskab.</param>
        /// <returns>Alle konti i regnskabet.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<KontoListeView> KontoGetByRegnskab(KontoGetByRegnskabQuery kontoGetByRegnskabQuery);

        /// <summary>
        /// Henter en given konto i et givent regnskab.
        /// </summary>
        /// <param name="kontoGetByRegnskabAndKontonummerQuery">Forespørgelse til at hente en given konto i et givent regnskab.</param>
        /// <returns>Konto.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        KontoView KontoGetByRegnskabAndKontonummer(KontoGetByRegnskabAndKontonummerQuery kontoGetByRegnskabAndKontonummerQuery);

        /// <summary>
        /// Henter alle budgetkonti i et givent regnskab.
        /// </summary>
        /// <param name="budgetkontoGetByRegnskabQuery">Forespørgelse til at hente alle budgetkonti i et givent regnskab.</param>
        /// <returns>Alle budgetkonti i et givent regnskab.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<BudgetkontoListeView> BudgetkontoGetByRegnskab(BudgetkontoGetByRegnskabQuery budgetkontoGetByRegnskabQuery);

        /// <summary>
        /// Henter en given budgetkonto i et givent regnskab.
        /// </summary>
        /// <param name="budgetkontoGetByRegnskabAndKontonummerQuery">Forespørgelse til at hente en given budgetkonto i et givent regnskab.</param>
        /// <returns>Budgetkonto.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BudgetkontoView BudgetkontoGetByRegnskabAndKontonummer(BudgetkontoGetByRegnskabAndKontonummerQuery budgetkontoGetByRegnskabAndKontonummerQuery);

        /// <summary>
        /// Henter alle bogføringslinjer for et givent regnskab.
        /// </summary>
        /// <param name="bogføringslinjeGetByRegnskabQuery">Forespørgelse til at hente alle bogføringslinjer for et givent regnskab.</param>
        /// <returns>Alle bogføringslinjer for regnskabet.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<BogføringslinjeView> BogføringslinjeGetByRegnskab(BogføringslinjeGetByRegnskabQuery bogføringslinjeGetByRegnskabQuery);

        /// <summary>
        /// Henter alle kontogrupper.
        /// </summary>
        /// <param name="kontogruppeGetAllQuery">Forespørgelse til at hente alle kontogrupper.</param>
        /// <returns>Alle kontogrupper.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<KontogruppeView> KontogruppeGetAll(KontogruppeGetAllQuery kontogruppeGetAllQuery);

        /// <summary>
        /// Henter en given kontogruppe.
        /// </summary>
        /// <param name="kontogruppeGetByNummerQuery">Forespørgelse tiol at hente en given kontogruppe.</param>
        /// <returns>Kontogruppe.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        KontogruppeView KontogruppeGetByNummer(KontogruppeGetByNummerQuery kontogruppeGetByNummerQuery);

        /// <summary>
        /// Henter alle grupper for budgetkonti.
        /// </summary>
        /// <param name="budgetkontogruppeGetAllQuery">Forespørgelse til at hente alle budgetkonti.</param>
        /// <returns>Alle grupper for budgetkonti.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<BudgetkontogruppeView> BudgetkontogruppeGetAll(BudgetkontogruppeGetAllQuery budgetkontogruppeGetAllQuery);

        /// <summary>
        /// Henter en given gruppe for budgetkonti.
        /// </summary>
        /// <param name="budgetkontogruppeGetByNummerQuery">Forespørgelse til at en given gruppe for budgetkonti.</param>
        /// <returns>Gruppe for budgetkonti.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BudgetkontogruppeView BudgetkontogruppeGetByNummer(BudgetkontogruppeGetByNummerQuery budgetkontogruppeGetByNummerQuery);
    }
}
