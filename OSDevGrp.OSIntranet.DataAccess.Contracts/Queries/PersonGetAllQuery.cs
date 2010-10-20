using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter alle personer.
    /// </summary>
    [DataContract(Name = "PersonGetAllQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class PersonGetAllQuery : IQuery
    {
    }
}
