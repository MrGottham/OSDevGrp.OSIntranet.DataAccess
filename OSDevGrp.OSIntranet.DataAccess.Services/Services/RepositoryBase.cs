using System.ServiceModel;
using OSDevGrp.OSIntranet.DataAccess.Contracts;

namespace OSDevGrp.OSIntranet.DataAccess.Services.Repositories
{
    /// <summary>
    /// Basisklasse for et repository.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, Namespace = SoapNamespaces.DataAccessNamespace)]
    public abstract class RepositoryBase
    {
    }
}
