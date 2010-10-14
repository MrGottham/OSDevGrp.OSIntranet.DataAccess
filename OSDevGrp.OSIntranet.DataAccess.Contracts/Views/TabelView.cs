using System.Runtime.Serialization;
using OSDevGrp.OSIntranet.CommonLibrary.Infrastructure.Interfaces;

namespace OSDevGrp.OSIntranet.DataAccess.Contracts.Views
{
    /// <summary>
    /// Viewmodel for generelle tabeloplysninger.
    /// </summary>
    [DataContract(Name = "Tabel", Namespace = SoapNamespaces.DataAccessNamespace)]
    public abstract class TabelView : IView
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
