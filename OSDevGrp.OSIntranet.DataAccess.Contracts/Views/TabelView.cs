using System.Runtime.Serialization;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewmodel for generelle tabeloplysinger.
    /// </summary>
    [DataContract(Name = "Tabel", Namespace = SoapNamespaces.DataAccessNamespace)]
    public abstract class TabelView
    {
        /// <summary>
        /// Nummer.
        /// </summary>
        [DataMember(IsRequired = true)]
        public int Nummer
        {
            get;
            set;
        }

        /// <summary>
        /// Navn.
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Navn
        {
            get;
            set;
        }
    }
}
