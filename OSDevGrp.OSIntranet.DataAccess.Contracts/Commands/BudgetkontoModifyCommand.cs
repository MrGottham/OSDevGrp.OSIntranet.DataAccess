using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til opdatering af en given budgetkonto.
    /// </summary>
    [DataContract(Name = "BudgetkontoModifyCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BudgetkontoModifyCommand : KontoModifyCommandBase
    {
        /// <summary>
        /// Unik identifikation af gruppen for budgetkontoen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Budgetkontogruppe
        {
            get;
            set;
        }
    }
}
