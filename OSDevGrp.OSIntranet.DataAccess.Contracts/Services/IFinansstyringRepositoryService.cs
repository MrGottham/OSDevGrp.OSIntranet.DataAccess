using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
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
        /// Henter alle regnskaber.
        /// </summary>
        /// <param name="regnskabGetAllQuery">Forespørgelse til at hente alle regnskaber.</param>
        /// <returns>Alle regnskaber.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<RegnskabListeView> RegnskabGetAll(RegnskabGetAllQuery regnskabGetAllQuery);

        /// <summary>
        /// Henter et givent regnskab.
        /// </summary>
        /// <param name="regnskabGetByNummerQuery">Forespørgelse til at hente et givent regnskab.</param>
        /// <returns>Regnskab.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        RegnskabView RegnskabGetByNummer(RegnskabGetByNummerQuery regnskabGetByNummerQuery);

        /// <summary>
        /// Henter alle konti i et givent regnskab.
        /// </summary>
        /// <param name="kontoGetByRegnskabQuery">Forespørgelse til at hente alle konti i et givent regnskab.</param>
        /// <returns>Alle konti i regnskabet.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<KontoListeView> KontoGetByRegnskab(KontoGetByRegnskabQuery kontoGetByRegnskabQuery);

        /// <summary>
        /// Henter en given konto i et givent regnskab.
        /// </summary>
        /// <param name="kontoGetByRegnskabAndKontonummerQuery">Forespørgelse til at hente en given konto i et givent regnskab.</param>
        /// <returns>Konto.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        KontoView KontoGetByRegnskabAndKontonummer(KontoGetByRegnskabAndKontonummerQuery kontoGetByRegnskabAndKontonummerQuery);

        /// <summary>
        /// Opdaterer eller tilføjer kreditoplysninger til en given konto.
        /// </summary>
        /// <param name="kreditoplysningerAddOrModifyCommand">Kommando til opdatering eller tilføjelse af kreditoplysninger til en given konto.</param>
        /// <returns>Opdateret eller tilføjet kreditoplysninger.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        KreditoplysningerView KreditoplysningerAddOrModify(KreditoplysningerAddOrModifyCommand kreditoplysningerAddOrModifyCommand);

        /// <summary>
        /// Opdaterer eller tilføjer én til flere kreditoplysninger til én eller flere konti.
        /// </summary>
        /// <param name="commands">Kommandoer til opdatering eller tilføjelse af kreditoplysninger til en given konto.</param>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void KreditoplysningerAddOrModifyMary(IEnumerable<KreditoplysningerAddOrModifyCommand> commands);

        /// <summary>
        /// Henter alle budgetkonti i et givent regnskab.
        /// </summary>
        /// <param name="budgetkontoGetByRegnskabQuery">Forespørgelse til at hente alle budgetkonti i et givent regnskab.</param>
        /// <returns>Alle budgetkonti i et givent regnskab.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<BudgetkontoListeView> BudgetkontoGetByRegnskab(BudgetkontoGetByRegnskabQuery budgetkontoGetByRegnskabQuery);

        /// <summary>
        /// Henter en given budgetkonto i et givent regnskab.
        /// </summary>
        /// <param name="budgetkontoGetByRegnskabAndKontonummerQuery">Forespørgelse til at hente en given budgetkonto i et givent regnskab.</param>
        /// <returns>Budgetkonto.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BudgetkontoView BudgetkontoGetByRegnskabAndKontonummer(BudgetkontoGetByRegnskabAndKontonummerQuery budgetkontoGetByRegnskabAndKontonummerQuery);

        /// <summary>
        /// Opdaterer eller tilføjer budgetoplysninger til en given budgetkonto.
        /// </summary>
        /// <param name="budgetoplysningerAddOrModifyCommand">Kommando til opdatering eller tilføjelse af budgetoplysninger til en given budgetkonto.</param>
        /// <returns>Opdateret eller tilføjet budgetoplysninger.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BudgetoplysningerView BudgetoplysningerAddOrModify(BudgetoplysningerAddOrModifyCommand budgetoplysningerAddOrModifyCommand);

        /// <summary>
        /// Opdaterer eller tilføjer én til flere budgetoplysninger til én eller flere budgetkonti.
        /// </summary>
        /// <param name="commands">Kommandoer til opdatering eller tilføjelse af budgetoplysninger til en given budgetkonto.</param>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void BudgetoplysningerAddOrModifyMany(IEnumerable<BudgetoplysningerAddOrModifyCommand> commands);

        /// <summary>
        /// Henter alle bogføringslinjer for et givent regnskab.
        /// </summary>
        /// <param name="bogføringslinjeGetByRegnskabQuery">Forespørgelse til at hente alle bogføringslinjer for et givent regnskab.</param>
        /// <returns>Alle bogføringslinjer for regnskabet.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<BogføringslinjeView> BogføringslinjeGetByRegnskab(BogføringslinjeGetByRegnskabQuery bogføringslinjeGetByRegnskabQuery);

        /// <summary>
        /// Tilføjer en bogføringslinje.
        /// </summary>
        /// <param name="bogføringslinjeAddCommand">Kommando til tilføjelse af en bogføringslinje.</param>
        /// <returns>Tilføjet bogføringslinje.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
         BogføringslinjeView BogføringslinjeAdd(BogføringslinjeAddCommand bogføringslinjeAddCommand);

        /// <summary>
        /// Henter alle kontogrupper.
        /// </summary>
        /// <param name="kontogruppeGetAllQuery">Forespørgelse til at hente alle kontogrupper.</param>
        /// <returns>Alle kontogrupper.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<KontogruppeView> KontogruppeGetAll(KontogruppeGetAllQuery kontogruppeGetAllQuery);

        /// <summary>
        /// Henter en given kontogruppe.
        /// </summary>
        /// <param name="kontogruppeGetByNummerQuery">Forespørgelse tiol at hente en given kontogruppe.</param>
        /// <returns>Kontogruppe.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        KontogruppeView KontogruppeGetByNummer(KontogruppeGetByNummerQuery kontogruppeGetByNummerQuery);

        /// <summary>
        /// Tilføjer en kontogruppe.
        /// </summary>
        /// <param name="kontogruppeAddCommand">Kommando til tilføjelse af en kontogruppe.</param>
        /// <returns>Tilføjet kontogruppe.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        KontogruppeView KontogruppeAdd(KontogruppeAddCommand kontogruppeAddCommand);

        /// <summary>
        /// Opdaterer en given kontogruppe.
        /// </summary>
        /// <param name="kontogruppeModifyCommand">Kommando til opdatering af en kontogruppe.</param>
        /// <returns>Opdateret kontogruppe.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        KontogruppeView KontogruppeModify(KontogruppeModifyCommand kontogruppeModifyCommand);

        /// <summary>
        /// Henter alle grupper for budgetkonti.
        /// </summary>
        /// <param name="budgetkontogruppeGetAllQuery">Forespørgelse til at hente alle budgetkonti.</param>
        /// <returns>Alle grupper for budgetkonti.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<BudgetkontogruppeView> BudgetkontogruppeGetAll(BudgetkontogruppeGetAllQuery budgetkontogruppeGetAllQuery);

        /// <summary>
        /// Henter en given gruppe for budgetkonti.
        /// </summary>
        /// <param name="budgetkontogruppeGetByNummerQuery">Forespørgelse til at en given gruppe for budgetkonti.</param>
        /// <returns>Gruppe for budgetkonti.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BudgetkontogruppeView BudgetkontogruppeGetByNummer(BudgetkontogruppeGetByNummerQuery budgetkontogruppeGetByNummerQuery);

        /// <summary>
        /// Tilføjer en gruppe til budgetkonti.
        /// </summary>
        /// <param name="budgetkontogruppeAddCommand">Kommando til tilføjelse af en gruppe til budgetkonti.</param>
        /// <returns>Tilføjet gruppe til budgetkonti.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BudgetkontogruppeView BudgetkontogruppeAdd(BudgetkontogruppeAddCommand budgetkontogruppeAddCommand);

        /// <summary>
        /// Opdaterer en given gruppe til budgetkonti.
        /// </summary>
        /// <param name="budgetkontogruppeModifyCommand">Kommand til opdatering af en given gruppe til budgetkonti.</param>
        /// <returns>Opdateret gruppe til budgetkonti.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BudgetkontogruppeView BudgetkontogruppeModify(BudgetkontogruppeModifyCommand budgetkontogruppeModifyCommand);
    }
}
