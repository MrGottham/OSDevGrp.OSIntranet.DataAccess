using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for et regnskab.
    /// </summary>
    [DataContract(Name = "Regnskab", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class RegnskabView : RegnskabListeView
    {
        /// <summary>
        /// Brevhoved.
        /// </summary>
        [DataMember(IsRequired = false)]
        public BrevhovedView Brevhoved
        {
            get;
            set;
        }

        /// <summary>
        /// Konti.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<KontoView> Konti
        {
            get;
            set;
        }

        /// <summary>
        /// Budgetkonti.
        /// </summary>
        [DataMember(IsRequired = true)]
        public IEnumerable<BudgetkontoView> Budgetkonti
        {
            get;
            set;
        }
    }
}
