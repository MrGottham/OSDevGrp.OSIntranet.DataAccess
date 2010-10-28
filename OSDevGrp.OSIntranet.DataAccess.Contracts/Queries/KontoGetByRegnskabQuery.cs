using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse alle konti på et givent regnskab.
    /// </summary>
    [DataContract(Name = "KontoGetByRegnskabQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class KontoGetByRegnskabQuery : IQuery
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
