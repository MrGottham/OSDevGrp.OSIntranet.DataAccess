using System.ServiceModel;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Services
{
    /// <summary>
    /// Servicekontrakt til repository for finansstyring.
    /// </summary>
    [ServiceContract(Name = SoapNamespaces.FinansstyringRepositoryServiceName, Namespace = SoapNamespaces.DataAccessNamespace)]
    public interface IFinansstyringRepositoryService : IRepositoryService
    {
        [OperationContract]
        void Test();
    }
}
