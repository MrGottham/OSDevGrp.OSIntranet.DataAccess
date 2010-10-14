using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter alle adressegrupper.
    /// </summary>
    [DataContract(Name = "AdressegruppeGetAllQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class AdressegruppeGetAllQuery : IQuery
    {
    }
}
