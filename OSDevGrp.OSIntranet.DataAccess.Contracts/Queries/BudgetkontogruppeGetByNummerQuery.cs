using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Queries
{
    /// <summary>
    /// Query til forespørgelse efter en given gruppe for budgetkonti.
    /// </summary>
    [DataContract(Name = "BudgetkontogruppeGetByNummerQuery", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BudgetkontogruppeGetByNummerQuery : IQuery
    {
        /// <summary>
        /// Unik identifikation af gruppen for budgetkonti.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }
    }
}
