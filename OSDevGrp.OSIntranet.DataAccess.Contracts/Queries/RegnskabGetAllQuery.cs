using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter alle regnskaber.
    /// </summary>
    [DataContract(Name = "RegnskabGetAllQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class RegnskabGetAllQuery : IQuery
    {
    }
}
