using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse en given konto i et givent regnskab.
    /// </summary>
    [DataContract(Name = "KontoGetByRegnskabAndKontonummerQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class KontoGetByRegnskabAndKontonummerQuery : IQuery
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

        /// <summary>
        /// Unik identifikation af kontoen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonummer
        {
            get;
            set;
        }
    }
}
