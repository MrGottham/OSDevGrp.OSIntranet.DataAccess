using System.Collections.Generic;
using System.ServiceModel;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Views;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til repository for adressekartoteket.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.AdresseRepositoryServiceName, Namespace = SoapNamespaces.DataAccessNamespace)]
    public interface IAdresseRepositoryService
    {
        /// <summary>
        /// Henter alle adressegrupper.
        /// </summary>
        /// <returns>Alle adressegrupper.</returns>
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        IList<AdressegruppeView> AdressegruppeGetAll();
    }
}
