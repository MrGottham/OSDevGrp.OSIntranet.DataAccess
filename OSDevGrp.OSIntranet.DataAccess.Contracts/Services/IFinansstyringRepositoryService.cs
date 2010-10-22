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
        BudgetkontogruppeView BudgetkontoGetByNummer(BudgetkontogruppeGetByNummerQuery budgetkontogruppeGetByNummerQuery);
    }
}
