using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse alle budgetkonti på et givent regnskab.
    /// </summary>
    [DataContract(Name = "BudgetkontoGetByRegnskabQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BudgetkontoGetByRegnskabQuery : IQuery
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
