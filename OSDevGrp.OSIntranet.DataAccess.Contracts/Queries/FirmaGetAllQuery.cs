using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter alle firmaer.
    /// </summary>
    [DataContract(Name = "FirmaGetAllQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class FirmaGetAllQuery : IQuery
    {
    }
}
