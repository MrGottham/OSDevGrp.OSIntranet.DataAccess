using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.DataAccess.Contracts.Enums;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en kontogruppe.
    /// </summary>
    [DataContract(Name = "Kontogruppe", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class KontogruppeView : TabelView
    {
        /// <summary>
        /// Kontogruppetype.
        /// </summary>
        [DataMember(IsRequired = true)]
        public KontogruppeType KontogruppeType
        {
            get;
            set;
        }
    }
}
