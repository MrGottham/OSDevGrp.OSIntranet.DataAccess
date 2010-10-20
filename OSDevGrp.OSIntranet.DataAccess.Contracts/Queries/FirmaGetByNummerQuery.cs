using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter et givent firma.
    /// </summary>
    [DataContract(Name = "FirmaGetByNummerQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class FirmaGetByNummerQuery : IQuery
    {
        /// <summary>
        /// Unik identifikation af firmaet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }
    }
}
