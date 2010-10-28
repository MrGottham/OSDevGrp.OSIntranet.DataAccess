using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en kontoliste.
    /// </summary>
    [DataContract(Name = "KontoListeView", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class KontoListeView : KontoListeViewBase
    {
        /// <summary>
        /// Kontogruppe.
        /// </summary>
        [DataMember(IsRequired = true)]
        public KontogruppeView Kontogruppe
        {
            get;
            set;
        }
    }
}
