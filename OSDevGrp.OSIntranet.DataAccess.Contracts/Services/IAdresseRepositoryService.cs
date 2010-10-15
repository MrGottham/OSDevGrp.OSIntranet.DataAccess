using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Queries;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til repository for adressekartoteket.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.AdresseRepositoryServiceName, Namespace = SoapNamespaces.DataAccessNamespace)]
    public interface IAdresseRepositoryService : IRepositoryService
    {
        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <param name="adressegruppeGetAllQuery">Query til forespørgelse efter alle adressegrupper.</param>
        /// <returns>Alle adressegrupper.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<AdressegruppeView> AdressegruppeGetAll(AdressegruppeGetAllQuery adressegruppeGetAllQuery);

        /// <summary>
        /// Henter en given adressegruppe.
        /// </summary>
        /// <param name="adressegruppeGetByNummerQuery">Query til forespørgelse efter en given adressegruppe.</param>
        /// <returns>Adressegruppe.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        AdressegruppeView AdressegruppeGetByNummer(AdressegruppeGetByNummerQuery adressegruppeGetByNummerQuery);

        /// <summary>
        /// Henter alle betalingsbetingelser.
        /// </summary>
        /// <param name="betalingsbetingelseGetAllQuery">Query til forespørgelse efter alle betalingsbetingelser.</param>
        /// <returns>Alle betalingsbetingelser.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<BetalingsbetingelseView> BetalingsbetingelseGetAll(BetalingsbetingelseGetAllQuery betalingsbetingelseGetAllQuery);

        /// <summary>
        /// Henter en given betalingsbetingelse.
        /// </summary>
        /// <param name="betalingsbetingelseGetByNummerQuery">Query til forespørgelse efter en given betalingsbetingelse.</param>
        /// <returns>Betalingsbetingelse.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        BetalingsbetingelseView BetalingsbetingelseGetByNummer(BetalingsbetingelseGetByNummerQuery betalingsbetingelseGetByNummerQuery);
    }
}
