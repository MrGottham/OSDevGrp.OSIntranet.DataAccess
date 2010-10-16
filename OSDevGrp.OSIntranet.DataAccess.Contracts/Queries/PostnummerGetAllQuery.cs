using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter alle postnumre.
    /// </summary>
    [DataContract(Name = "PostnummerGetAllQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class PostnummerGetAllQuery : IQuery
    {
    }
}
