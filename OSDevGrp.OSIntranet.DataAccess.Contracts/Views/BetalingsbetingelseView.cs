using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en betalingsbetingelse.
    /// </summary>
    [DataContract(Name = "Betalingsbetingelse", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BetalingsbetingelseView : TabelView
    {
    }
}
