using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter alle kontogrupper.
    /// </summary>
    [DataContract(Name = "KontogruppeGetAllQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class KontogruppeGetAllQuery : IQuery
    {
    }
}
