using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Commands
{
    /// <summary>
    /// Command til oprettelse af en budgetkonto.
    /// </summary>
    [DataContract(Name = "BudgetkontoAddCommand", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BudgetkontoAddCommand : KontoAddCommandBase
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
