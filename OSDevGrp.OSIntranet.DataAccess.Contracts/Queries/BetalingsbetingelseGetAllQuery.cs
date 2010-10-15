using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter alle betalingsbetingelser.
    /// </summary>
    [DataContract(Name = "BetalingsbetingelseGetAllQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BetalingsbetingelseGetAllQuery : IQuery
    {
    }
}
