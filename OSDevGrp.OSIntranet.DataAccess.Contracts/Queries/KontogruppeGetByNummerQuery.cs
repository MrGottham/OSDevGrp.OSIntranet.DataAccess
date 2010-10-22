using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse en given kontogruppe.
    /// </summary>
    [DataContract(Name = "KontogruppeGetByNummerQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class KontogruppeGetByNummerQuery : IQuery
    {
        /// <summary>
        /// Unik identifikation af kontogruppen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }
    }
}
