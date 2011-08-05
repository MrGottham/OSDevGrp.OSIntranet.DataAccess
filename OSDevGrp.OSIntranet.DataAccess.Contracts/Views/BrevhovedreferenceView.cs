using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en reference til et brevhoved.
    /// </summary>
    [DataContract(Name = "Brevhovedreference", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BrevhovedreferenceView : TabelView
    {
    }
}
