using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Commands;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til repository for fælles elementer.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.FællesRepositoryServiceName, Namespace = SoapNamespaces.DataAccessNamespace)]
    public interface IFællesRepositoryService : IRepositoryService
    {
        /// <summary>
        /// Henter alle brevhoveder.
        /// </summary>
        /// <param name="brevhovedGetAllQuery">Query til forespørgelse efter alle brevhoveder.</param>
        /// <returns>Alle brevhoveder.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IEnumerable<BrevhovedView> BrevhovedGetAll(BrevhovedGetAllQuery brevhovedGetAllQuery);

        /// <summary>
        /// Henter et givent brevhoved.
        /// </summary>
        /// <param name="brevhovedGetByNummerQuery">Query til forespørgelse efter et givent brevhovede.</param>
        /// <returns>Brevhoved.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BrevhovedView BrevhovedGetByNummer(BrevhovedGetByNummerQuery brevhovedGetByNummerQuery);

        /// <summary>
        /// Tilføjer et brevhoved.
        /// </summary>
        /// <param name="brevhovedAddCommand">Command til oprettelse af et brevhoved.</param>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void BrevhovedAdd(BrevhovedAddCommand brevhovedAddCommand);

        /// <summary>
        /// Opdaterer et brevhoved.
        /// </summary>
        /// <param name="brevhovedModifyCommand">Command til opdatering af et givent brevhoved.</param>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void BrevhovedModify(BrevhovedModifyCommand brevhovedModifyCommand);
    }
}
