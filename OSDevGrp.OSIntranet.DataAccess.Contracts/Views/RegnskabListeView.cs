using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en liste af regnskaber.
    /// </summary>
    [DataContract(Name = "Regnskabsliste", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class RegnskabListeView : TabelView
    {
        /// <summary>
        /// Reference til et brevhoved.
        /// </summary>
        [DataMember(IsRequired = false)]
        public BrevhovedreferenceView Brevhoved
        {
            get;
            set;
        }
    }
}
