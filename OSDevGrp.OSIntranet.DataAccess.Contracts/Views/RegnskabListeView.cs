using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en liste af regnskaber.
    /// </summary>
    [DataContract(Name = "RegnskabListeView", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class RegnskabListeView : TabelView
    {
    }
}
