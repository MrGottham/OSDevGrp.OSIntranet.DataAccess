using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en given betalingsbetingelse.
    /// </summary>
    [DataContract(Name = "BetalingsbetingelseGetByNummerQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BetalingsbetingelseGetByNummerQuery : IQuery
    {
        /// <summary>
        /// Nummer på betalingsbetingelsen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }
    }
}
