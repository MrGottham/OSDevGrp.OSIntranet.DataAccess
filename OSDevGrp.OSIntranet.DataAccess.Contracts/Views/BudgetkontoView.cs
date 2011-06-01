using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en budgetkonto.
    /// </summary>
    [DataContract(Name = "Budgetkonto", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BudgetkontoView : BudgetkontoListeView
    {
        /// <summary>
        /// Budgetoplysninger.
        /// </summary>
        [DataMember(IsRequired = false)]
        public IEnumerable<BudgetoplysningerView> Budgetoplysninger
        {
            get;
            set;
        }

        /// <summary>
        /// Bogføringslinjer.
        /// </summary>
        [DataMember(IsRequired = false)]
        public IEnumerable<BogføringslinjeView> Bogføringslinjer
        {
            get;
            set;
        }
    }
}
