using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter alle adresser til en adresseliste.
    /// </summary>
    [DataContract(Name = "AdresselisteGetAllQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class AdresselisteGetAllQuery : IQuery
    {
    }
}
