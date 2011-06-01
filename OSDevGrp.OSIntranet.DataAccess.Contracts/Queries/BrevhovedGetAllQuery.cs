using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter alle brevhoveder.
    /// </summary>
    [DataContract(Name = "BrevhovedGetAllQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BrevhovedGetAllQuery : IQuery
    {
    }
}
