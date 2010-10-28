using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse en given budgetkonto i et givent regnskab.
    /// </summary>
    [DataContract(Name = "BudgetkontoGetByRegnskabAndKontonummerQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BudgetkontoGetByRegnskabAndKontonummerQuery : IQuery
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
        /// Unik identifikation af budgetkontoen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Kontonummer
        {
            get;
            set;
        }
    }
}
