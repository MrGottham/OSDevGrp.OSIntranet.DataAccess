using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en liste af budgetkonti.
    /// </summary>
    [DataContract(Name = "BudgetkontoListeView", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BudgetkontoListeView : KontoListeViewBase
    {
        /// <summary>
        /// Budgetkontogruppe.
        /// </summary>
        [DataMember(IsRequired = true)]
        public BudgetkontogruppeView Budgetkontogruppe
        {
            get;
            set;
        }
    }
}
