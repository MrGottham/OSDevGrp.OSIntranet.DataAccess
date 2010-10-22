using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en gruppe af budgetkonti.
    /// </summary>
    [DataContract(Name = "Budgetkontogruppe", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class BudgetkontogruppeView : TabelView
    {
    }
}
