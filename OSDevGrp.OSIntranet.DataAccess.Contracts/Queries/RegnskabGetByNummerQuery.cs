using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter et givent regnskab.
    /// </summary>
    [DataContract(Name = "RegnskabGetByNummerQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class RegnskabGetByNummerQuery : IQuery
    {
        /// <summary>
        /// Unik identifikation af regnskabet.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Regnskabsnummer
        {
            get;
            set;
        }
    }
}
