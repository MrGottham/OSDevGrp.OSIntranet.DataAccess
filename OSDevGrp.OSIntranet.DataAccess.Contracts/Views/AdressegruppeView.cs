using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en adressegruppe.
    /// </summary>
    [DataContract(Name = "Adressegruppe", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class AdressegruppeView : TabelView
    {
        /// <summary>
        /// Nummer på den tilsvarende adressegruppe i OSWEBDB.
        /// </summary>
        [DataMember(IsRequired = false)]
        public int AdressegruppeOswebdb
        {
            get;
            set;
        }
    }
}