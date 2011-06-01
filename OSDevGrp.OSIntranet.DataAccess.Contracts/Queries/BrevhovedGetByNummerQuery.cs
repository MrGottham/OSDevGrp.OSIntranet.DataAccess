using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter et givent brevhoved.
    /// </summary>
    [DataContract(Name = "BrevhovedGetByNummerQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BrevhovedGetByNummerQuery : IQuery
    {
        /// <summary>
        /// Nummer på brevhovedet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }
    }
}
