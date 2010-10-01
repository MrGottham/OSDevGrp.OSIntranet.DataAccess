using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewobjekt for en adressegruppe.
    /// </summary>
    [DataContract(Name = "Adressegruppe", Namespace = SoapNamespaces.DataAccessNamespace)]
    public class AdressegruppeView
    {
        /// <summary>
        /// Nummer på adressegruppen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn på adressegruppen.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }

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